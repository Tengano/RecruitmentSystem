using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using RecruitmentSystem.Services;
using RecruitmentSystem.Filters;

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

        public IActionResult Login()
        {
            return View();
        }

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

        public IActionResult Register()
        {
            return View();
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đã đăng xuất thành công!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var user = await _authService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng!";
                return RedirectToAction("Login");
            }

            var totalApplications = await _context.Applications
                .Where(a => a.Email == user.Email)
                .CountAsync();

            var successfulApplications = await _context.Applications
                .Where(a => a.Email == user.Email && a.TrangThai == "Đã chấp nhận")
                .CountAsync();

            var pendingApplications = await _context.Applications
                .Where(a => a.Email == user.Email && a.TrangThai == "Chờ xem xét")
                .CountAsync();

            var recentApplications = await _context.Applications
                .Include(a => a.CongViec)
                .Where(a => a.Email == user.Email)
                .OrderByDescending(a => a.NgayUngTuyen)
                .Take(5)
                .ToListAsync();

            var contactInfo = await _context.ContactInfos
                .FirstOrDefaultAsync(c => c.UserId == userId.Value);

            var educations = await _context.Educations
                .Where(e => e.UserId == userId.Value)
                .OrderByDescending(e => e.NgayBatDau)
                .ToListAsync();

            var experiences = await _context.Experiences
                .Where(e => e.UserId == userId.Value)
                .OrderByDescending(e => e.NgayBatDau)
                .ToListAsync();

            var skills = await _context.Skills
                .Where(s => s.UserId == userId.Value)
                .OrderBy(s => s.DanhMuc)
                .ThenBy(s => s.TenKyNang)
                .ToListAsync();

            ViewBag.TotalApplications = totalApplications;
            ViewBag.SuccessfulApplications = successfulApplications;
            ViewBag.PendingApplications = pendingApplications;
            ViewBag.RecentApplications = recentApplications;
            ViewBag.ContactInfo = contactInfo;
            ViewBag.Educations = educations;
            ViewBag.Experiences = experiences;
            ViewBag.Skills = skills;

            return View(user);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập địa chỉ email!";
                return View();
            }

            var result = await _authService.GeneratePasswordResetTokenAsync(email);
            if (result)
            {
                var user = await _authService.GetUserByEmailAsync(email);
                if (user != null)
                {
                    TempData["SuccessMessage"] = $"Mã reset mật khẩu của bạn là: {user.MaDatLaiMatKhau}. Mã này có hiệu lực trong 24 giờ.";
                    TempData["ResetToken"] = user.MaDatLaiMatKhau;
                    return RedirectToAction("ResetPassword");
                }
            }
            
            TempData["ErrorMessage"] = "Không tìm thấy tài khoản với email này!";
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string token, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            if (newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất 6 ký tự!";
                return View();
            }

            var result = await _authService.ResetPasswordAsync(token, newPassword);
            if (result)
            {
                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Mã reset không hợp lệ hoặc đã hết hạn!";
            return View();
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            oldPassword = oldPassword?.Trim() ?? "";
            newPassword = newPassword?.Trim() ?? "";
            confirmPassword = confirmPassword?.Trim() ?? "";

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới xác nhận không khớp!";
                return View();
            }

            if (newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới phải có ít nhất 6 ký tự!";
                return View();
            }

            if (oldPassword == newPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới phải khác mật khẩu cũ!";
                return View();
            }

            var result = await _authService.ChangePasswordAsync(userId.Value, oldPassword, newPassword);
            if (result)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Profile");
            }

            TempData["ErrorMessage"] = "Mật khẩu cũ không đúng!";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var user = await _authService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng!";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(User user)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(user.HoTen) || string.IsNullOrEmpty(user.Email))
            {
                TempData["ErrorMessage"] = "Họ tên và email là bắt buộc!";
                return View(user);
            }

            var existingUser = await _authService.GetUserByEmailAsync(user.Email);
            if (existingUser != null && existingUser.Id != userId.Value)
            {
                TempData["ErrorMessage"] = "Email đã được sử dụng bởi người dùng khác!";
                return View(user);
            }

            var currentUser = await _authService.GetUserByIdAsync(userId.Value);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng!";
                return RedirectToAction("Login");
            }

            currentUser.HoTen = user.HoTen;
            currentUser.Email = user.Email;

            try
            {
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("UserName", currentUser.HoTen);
                
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                return RedirectToAction("Profile");
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật hồ sơ!";
                return View(user);
            }
        }
    }
}