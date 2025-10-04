using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;

namespace RecruitmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalJobs = await _context.Jobs.CountAsync();
            ViewBag.ActiveJobs = await _context.Jobs.Where(j => j.IsActive).CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.PendingApplications = await _context.Applications.Where(a => a.Status == "Pending").CountAsync();
            ViewBag.TotalCandidates = await _context.Candidates.CountAsync();

            var recentApplications = await _context.Applications
                .Include(a => a.Job)
                .OrderByDescending(a => a.AppliedDate)
                .Take(10)
                .ToListAsync();

            return View(recentApplications);
        }

        // Jobs Management
        public async Task<IActionResult> Jobs()
        {
            var jobs = await _context.Jobs.OrderByDescending(j => j.PostedDate).ToListAsync();
            return View(jobs);
        }

        // GET: Admin/CreateJob
        public IActionResult CreateJob()
        {
            return View();
        }

        // POST: Admin/CreateJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob(Job job)
        {
            if (ModelState.IsValid)
            {
                job.PostedDate = DateTime.Now;
                _context.Add(job);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo tin tuyển dụng thành công!";
                return RedirectToAction(nameof(Jobs));
            }
            return View(job);
        }

        // GET: Admin/EditJob/5
        public async Task<IActionResult> EditJob(int? id)
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
            return View(job);
        }

        // POST: Admin/EditJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJob(int id, Job job)
        {
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tin tuyển dụng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.JobId))
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
            return View(job);
        }

        // GET: Admin/DeleteJob/5
        public async Task<IActionResult> DeleteJob(int? id)
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

            return View(job);
        }

        // POST: Admin/DeleteJob/5
        [HttpPost, ActionName("DeleteJob")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJobConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa tin tuyển dụng thành công!";
            }

            return RedirectToAction(nameof(Jobs));
        }

        // Applications Management
        public async Task<IActionResult> Applications()
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();
            return View(applications);
        }

        // GET: Admin/ApplicationDetails/5
        public async Task<IActionResult> ApplicationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Admin/UpdateApplicationStatus
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int id, string status)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                application.Status = status;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển!" });
        }

        // Candidates Management
        public async Task<IActionResult> Candidates()
        {
            var candidates = await _context.Candidates
                .OrderByDescending(c => c.RegisteredDate)
                .ToListAsync();
            return View(candidates);
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}

