using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel ThongTinDauVao { get; set; }

        public string UrlTraVe { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email là bắt buộc")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
            [DataType(DataType.Password)]
            public string MatKhau { get; set; }

            [Display(Name = "Ghi nhớ đăng nhập")]
            public bool GhiNhoDangNhap { get; set; }
        }

        public async Task OnGetAsync(string urlTraVe = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            urlTraVe ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            UrlTraVe = urlTraVe;
        }

        public async Task<IActionResult> OnPostAsync(string urlTraVe = null)
        {
            urlTraVe ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var ketQua = await _signInManager.PasswordSignInAsync(ThongTinDauVao.Email, ThongTinDauVao.MatKhau, ThongTinDauVao.GhiNhoDangNhap, lockoutOnFailure: false);
                if (ketQua.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã đăng nhập thành công.");
                    return LocalRedirect(urlTraVe);
                }
                if (ketQua.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = urlTraVe, RememberMe = ThongTinDauVao.GhiNhoDangNhap });
                }
                if (ketQua.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản người dùng đã bị khóa.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công. Vui lòng kiểm tra lại email và mật khẩu.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
