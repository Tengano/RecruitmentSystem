using Microsoft.AspNetCore.Mvc;
using RecruitmentSystem.Models;
using RecruitmentSystem.Services;

namespace RecruitmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
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
                user.VaiTro = "User"; // Mặc định là User
                user.HoatDong = true;
                user.NgayTao = DateTime.Now;

                var result = await _authService.RegisterAsync(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
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
    }
}
