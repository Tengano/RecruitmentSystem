using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using RecruitmentSystem.Services;
using RecruitmentSystem.Filters;

namespace RecruitmentSystem.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authService;

        public AdminController(ApplicationDbContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TotalJobs = await _context.Jobs.CountAsync();
                ViewBag.ActiveJobs = await _context.Jobs.Where(j => j.HoatDong == true).CountAsync();
                ViewBag.TotalApplications = await _context.Applications.CountAsync();
                ViewBag.PendingApplications = await _context.Applications.Where(a => a.TrangThai == "Chờ xem xét").CountAsync();
                ViewBag.TotalCandidates = await _context.Candidates.CountAsync();

                var donUngTuyenGanDay = await _context.Applications
                    .Include(a => a.CongViec)
                    .Where(a => a.CongViec != null)
                    .OrderByDescending(a => a.NgayUngTuyen)
                    .Take(10)
                    .ToListAsync();

                if (TempData["SuccessMessage"] == null && TempData["ErrorMessage"] == null)
                {
                    TempData["SuccessMessage"] = "Chào mừng Quản trị viên hệ thống! Chúc bạn một ngày làm việc hiệu quả!";
                }

                return View(donUngTuyenGanDay);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu dashboard: {ex.Message}";
                
                ViewBag.TotalJobs = 0;
                ViewBag.ActiveJobs = 0;
                ViewBag.TotalApplications = 0;
                ViewBag.PendingApplications = 0;
                ViewBag.TotalCandidates = 0;

                return View(new List<Application>());
            }
        }

        public async Task<IActionResult> Jobs()
        {
            var congViec = await _context.Jobs.OrderByDescending(j => j.NgayDang).ToListAsync();
            return View(congViec);
        }

        public IActionResult CreateJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob(Job congViec)
        {
            if (ModelState.IsValid)
            {
                congViec.NgayDang = DateTime.Now;
                _context.Add(congViec);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo tin tuyển dụng thành công!";
                return RedirectToAction(nameof(Jobs));
            }
            return View(congViec);
        }

        public async Task<IActionResult> EditJob(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congViec = await _context.Jobs.FindAsync(id);
            if (congViec == null)
            {
                return NotFound();
            }
            return View(congViec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJob(int id, Job congViec)
        {
            if (id != congViec.MaCongViec)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congViec);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tin tuyển dụng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(congViec.MaCongViec))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Jobs));
            }
            return View(congViec);
        }

        public async Task<IActionResult> DeleteJob(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congViec = await _context.Jobs
                .FirstOrDefaultAsync(m => m.MaCongViec == id);
            if (congViec == null)
            {
                return NotFound();
            }

            return View(congViec);
        }

        [HttpPost, ActionName("DeleteJob")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJobConfirmed(int id)
        {
            try
            {
                var congViec = await _context.Jobs.FindAsync(id);
                if (congViec != null)
                {
                    var relatedApplications = await _context.Applications
                        .Where(a => a.MaCongViec == id)
                        .ToListAsync();

                    if (relatedApplications.Any())
                    {
                        _context.Applications.RemoveRange(relatedApplications);
                    }

                    _context.Jobs.Remove(congViec);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa tin tuyển dụng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tin tuyển dụng!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa tin tuyển dụng: {ex.Message}";
            }

            return RedirectToAction(nameof(Jobs));
        }

        public async Task<IActionResult> Applications()
        {
            try
            {
                var donUngTuyen = await _context.Applications
                    .Include(a => a.CongViec)
                    .Where(a => a.CongViec != null)
                    .OrderBy(a => a.MaDonUngTuyen)
                    .ToListAsync();
                return View(donUngTuyen);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách đơn ứng tuyển: {ex.Message}";
                return View(new List<Application>());
            }
        }

        public async Task<IActionResult> ApplicationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donUngTuyen = await _context.Applications
                .Include(a => a.CongViec)
                .FirstOrDefaultAsync(m => m.MaDonUngTuyen == id);

            if (donUngTuyen == null)
            {
                return NotFound();
            }

            return View(donUngTuyen);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int id, string trangThai)
        {
            var donUngTuyen = await _context.Applications.FindAsync(id);
            if (donUngTuyen != null)
            {
                donUngTuyen.TrangThai = trangThai;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển!" });
        }

        public async Task<IActionResult> Candidates()
        {
            var ungVien = await _context.Candidates
                .OrderBy(c => c.MaUngVien)
                .ToListAsync();
            return View(ungVien);
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.MaCongViec == id);
        }
    }
}