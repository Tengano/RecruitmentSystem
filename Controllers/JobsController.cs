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

        public async Task<IActionResult> Index(string? category, string? location, string? jobType, string? search)
        {
            var congViec = _context.Jobs.Where(j => j.HoatDong).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                congViec = congViec.Where(j => j.DanhMuc == category);
                ViewBag.SelectedCategory = category;
            }

            if (!string.IsNullOrEmpty(location))
            {
                congViec = congViec.Where(j => j.DiaDiem.Contains(location));
                ViewBag.SelectedLocation = location;
            }

            if (!string.IsNullOrEmpty(jobType))
            {
                congViec = congViec.Where(j => j.LoaiCongViec == jobType);
                ViewBag.SelectedJobType = jobType;
            }

            if (!string.IsNullOrEmpty(search))
            {
                congViec = congViec.Where(j => j.TieuDe.Contains(search) || j.CongTy.Contains(search) || j.MoTa.Contains(search));
                ViewBag.SearchTerm = search;
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

            congViec.LuotXem++;
            await _context.SaveChangesAsync();

            return View(congViec);
        }

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
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Application donUngTuyen, IFormFile? hoSo)
        {
            if (ModelState.IsValid)
            {

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


                    donUngTuyen.KyNang += $" | File đính kèm: /uploads/resumes/{tenFileDuyNhat}";
                }

                donUngTuyen.NgayUngTuyen = DateTime.Now;
                donUngTuyen.TrangThai = "Chờ xem xét";

                _context.Add(donUngTuyen);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đã gửi đơn ứng tuyển thành công! Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.";
                return RedirectToAction(nameof(Index));
            }

            var congViec = await _context.Jobs.FindAsync(donUngTuyen.MaCongViec);
            ViewBag.Job = congViec;
            return View(donUngTuyen);
        }
    }
}