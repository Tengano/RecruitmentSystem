using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using System.Diagnostics;

namespace RecruitmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var congViecNoiBat = await _context.Jobs
                .Where(j => j.HoatDong)
                .OrderByDescending(j => j.NgayDang)
                .Take(6)
                .ToListAsync();

            ViewBag.TotalJobs = await _context.Jobs.Where(j => j.HoatDong).CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.TotalCandidates = await _context.Candidates.CountAsync();

            return View(congViecNoiBat);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lưu vào database
                    _context.Contacts.Add(model);
                    await _context.SaveChangesAsync();
                    
                    // Log thông tin liên hệ
                    _logger.LogInformation($"Liên hệ mới từ: {model.HoTen} ({model.Email}) - {model.SoDienThoai}");
                    _logger.LogInformation($"Nội dung: {model.NoiDung}");
                    
                    TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất có thể.";
                    return RedirectToAction(nameof(Contact));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lưu thông tin liên hệ");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi thông tin liên hệ. Vui lòng thử lại sau.";
                    return View(model);
                }
            }
            
            return View(model);
        }

        public IActionResult AnimationDemo()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}