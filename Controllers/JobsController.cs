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
        public async Task<IActionResult> Index(string? category, string? location, string? jobType, string? search)
        {
            var jobs = _context.Jobs.Where(j => j.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                jobs = jobs.Where(j => j.Category == category);
                ViewBag.SelectedCategory = category;
            }

            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Location.Contains(location));
                ViewBag.SelectedLocation = location;
            }

            if (!string.IsNullOrEmpty(jobType))
            {
                jobs = jobs.Where(j => j.JobType == jobType);
                ViewBag.SelectedJobType = jobType;
            }

            if (!string.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j => j.Title.Contains(search) || j.Company.Contains(search) || j.Description.Contains(search));
                ViewBag.SearchTerm = search;
            }

            ViewBag.Categories = await _context.Jobs
                .Where(j => j.IsActive)
                .Select(j => j.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Locations = await _context.Jobs
                .Where(j => j.IsActive)
                .Select(j => j.Location)
                .Distinct()
                .ToListAsync();

            return View(await jobs.OrderByDescending(j => j.PostedDate).ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.JobId == id);

            if (job == null)
            {
                return NotFound();
            }

            // Increment view count
            job.Views++;
            await _context.SaveChangesAsync();

            return View(job);
        }

        // GET: Jobs/Apply/5
        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            ViewBag.Job = job;
            var application = new Application { JobId = job.JobId };
            return View(application);
        }

        // POST: Jobs/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Application application, IFormFile? resume)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (resume != null && resume.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "resumes");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + resume.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resume.CopyToAsync(fileStream);
                    }

                    application.ResumeUrl = "/uploads/resumes/" + uniqueFileName;
                }

                application.AppliedDate = DateTime.Now;
                application.Status = "Pending";

                _context.Add(application);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ứng tuyển thành công! Chúng tôi sẽ liên hệ với bạn sớm.";
                return RedirectToAction(nameof(Index));
            }

            var job = await _context.Jobs.FindAsync(application.JobId);
            ViewBag.Job = job;
            return View(application);
        }
    }
}

