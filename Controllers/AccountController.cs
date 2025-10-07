using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using RecruitmentSystem.Services;

namespace RecruitmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ApplicationDbContext _context;

        public AccountController(IAuthenticationService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var user = await _authService.LoginAsync(tenDangNhap, matKhau);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.HoTen);
                HttpContext.Session.SetString("UserRole", user.VaiTro);
                
                TempData["SuccessMessage"] = $"Chào mừng {user.HoTen}!";
                
                if (user.VaiTro == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.VaiTro = "User"; 
                user.HoatDong = true;
                user.NgayTao = DateTime.Now;

                var result = await _authService.RegisterAsync(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Tài khoản đã được đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập hoặc email đã tồn tại!";
                }
            }
            return View(user);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đã đăng xuất thành công!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem hồ sơ!";
                return RedirectToAction("Login");
            }

            var user = await _authService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng!";
                return RedirectToAction("Login");
            }

            // Lấy thống kê ứng tuyển
            var totalApplications = await _context.Applications
                .Where(a => a.Email == user.Email)
                .CountAsync();

            var successfulApplications = await _context.Applications
                .Where(a => a.Email == user.Email && a.TrangThai == "Đã chấp nhận")
                .CountAsync();

            var pendingApplications = await _context.Applications
                .Where(a => a.Email == user.Email && a.TrangThai == "Chờ xem xét")
                .CountAsync();

            // Lấy đơn ứng tuyển gần đây
            var recentApplications = await _context.Applications
                .Include(a => a.CongViec)
                .Where(a => a.Email == user.Email)
                .OrderByDescending(a => a.NgayUngTuyen)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalApplications = totalApplications;
            ViewBag.SuccessfulApplications = successfulApplications;
            ViewBag.PendingApplications = pendingApplications;
            ViewBag.RecentApplications = recentApplications;

            return View(user);
        }
    }
}
