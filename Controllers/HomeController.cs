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
            var featuredJobs = await _context.Jobs
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.PostedDate)
                .Take(6)
                .ToListAsync();

            ViewBag.TotalJobs = await _context.Jobs.Where(j => j.IsActive).CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.TotalCandidates = await _context.Candidates.Where(c => c.IsActive).CountAsync();

            return View(featuredJobs);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
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

