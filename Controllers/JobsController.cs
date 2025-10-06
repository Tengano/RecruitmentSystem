using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;

namespace RecruitmentSystem.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index(string? danhMuc, string? diaDiem, string? loaiCongViec, string? timKiem)
        {
            var congViec = _context.Jobs.Where(j => j.HoatDong).AsQueryable();

            if (!string.IsNullOrEmpty(danhMuc))
            {
                congViec = congViec.Where(j => j.DanhMuc == danhMuc);
                ViewBag.SelectedCategory = danhMuc;
            }

            if (!string.IsNullOrEmpty(diaDiem))
            {
                congViec = congViec.Where(j => j.DiaDiem.Contains(diaDiem));
                ViewBag.SelectedLocation = diaDiem;
            }

            if (!string.IsNullOrEmpty(loaiCongViec))
            {
                congViec = congViec.Where(j => j.LoaiCongViec == loaiCongViec);
                ViewBag.SelectedJobType = loaiCongViec;
            }

            if (!string.IsNullOrEmpty(timKiem))
            {
                congViec = congViec.Where(j => j.TieuDe.Contains(timKiem) || j.CongTy.Contains(timKiem) || j.MoTa.Contains(timKiem));
                ViewBag.SearchTerm = timKiem;
            }

            ViewBag.Categories = await _context.Jobs
                .Where(j => j.HoatDong)
                .Select(j => j.DanhMuc)
                .Distinct()
                .ToListAsync();

            ViewBag.Locations = await _context.Jobs
                .Where(j => j.HoatDong)
                .Select(j => j.DiaDiem)
                .Distinct()
                .ToListAsync();

            return View(await congViec.OrderByDescending(j => j.NgayDang).ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
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

            // Tăng số lượt xem
            congViec.LuotXem++;
            await _context.SaveChangesAsync();

            return View(congViec);
        }

        // GET: Jobs/Apply/5
        public async Task<IActionResult> Apply(int? id)
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

            ViewBag.Job = congViec;
            var donUngTuyen = new Application { MaCongViec = congViec.MaCongViec };
            return View(donUngTuyen);
        }

        // POST: Jobs/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Application donUngTuyen, IFormFile? hoSo)
        {
            if (ModelState.IsValid)
            {
                // Xử lý tải file (nếu cần)
                if (hoSo != null && hoSo.Length > 0)
                {
                    var thuMucTaiLen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "resumes");
                    Directory.CreateDirectory(thuMucTaiLen);

                    var tenFileDuyNhat = Guid.NewGuid().ToString() + "_" + hoSo.FileName;
                    var duongDanFile = Path.Combine(thuMucTaiLen, tenFileDuyNhat);

                    using (var fileStream = new FileStream(duongDanFile, FileMode.Create))
                    {
                        await hoSo.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn file vào Skills hoặc Experience nếu cần
                    donUngTuyen.KyNang += $" | File đính kèm: /uploads/resumes/{tenFileDuyNhat}";
                }

                donUngTuyen.NgayUngTuyen = DateTime.Now;
                donUngTuyen.TrangThai = "Chờ xem xét";

                _context.Add(donUngTuyen);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đã gửi đơn ứng tuyển thành công! Chúng tôi sẽ liên hệ với bạn sớm.";
                return RedirectToAction(nameof(Index));
            }

            var congViec = await _context.Jobs.FindAsync(donUngTuyen.MaCongViec);
            ViewBag.Job = congViec;
            return View(donUngTuyen);
        }
    }
}