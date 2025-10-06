using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using RecruitmentSystem.Services;

namespace RecruitmentSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authService;

        public AdminController(ApplicationDbContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        // Kiểm tra quyền admin
        private bool IsAdmin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return false;
            
            var user = _authService.GetUserByIdAsync(userId.Value).Result;
            return user?.VaiTro == "Admin";
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Bạn không có quyền quản trị để truy cập trang này!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TotalJobs = await _context.Jobs.CountAsync();
            ViewBag.ActiveJobs = await _context.Jobs.Where(j => j.HoatDong).CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.PendingApplications = await _context.Applications.Where(a => a.TrangThai == "Chờ xem xét").CountAsync();
            ViewBag.TotalCandidates = await _context.Candidates.CountAsync();

            var donUngTuyenGanDay = await _context.Applications
                .Include(a => a.CongViec)
                .OrderByDescending(a => a.NgayUngTuyen)
                .Take(10)
                .ToListAsync();

            return View(donUngTuyenGanDay);
        }

        // Jobs Management
        public async Task<IActionResult> Jobs()
        {
            var congViec = await _context.Jobs.OrderByDescending(j => j.NgayDang).ToListAsync();
            return View(congViec);
        }

        // GET: Admin/CreateJob
        public IActionResult CreateJob()
        {
            return View();
        }

        // POST: Admin/CreateJob
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

        // GET: Admin/EditJob/5
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

        // POST: Admin/EditJob/5
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

        // GET: Admin/DeleteJob/5
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

        // POST: Admin/DeleteJob/5
        [HttpPost, ActionName("DeleteJob")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJobConfirmed(int id)
        {
            var congViec = await _context.Jobs.FindAsync(id);
            if (congViec != null)
            {
                _context.Jobs.Remove(congViec);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa tin tuyển dụng thành công!";
            }

            return RedirectToAction(nameof(Jobs));
        }

        // Applications Management
        public async Task<IActionResult> Applications()
        {
            var donUngTuyen = await _context.Applications
                .Include(a => a.CongViec)
                .OrderByDescending(a => a.NgayUngTuyen)
                .ToListAsync();
            return View(donUngTuyen);
        }

        // GET: Admin/ApplicationDetails/5
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

        // POST: Admin/UpdateApplicationStatus
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

        // Candidates Management
        public async Task<IActionResult> Candidates()
        {
            var ungVien = await _context.Candidates
                .OrderByDescending(c => c.NgayTao)
                .ToListAsync();
            return View(ungVien);
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.MaCongViec == id);
        }
    }
}