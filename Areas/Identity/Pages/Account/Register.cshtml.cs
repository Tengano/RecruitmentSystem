using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel ThongTinDauVao { get; set; }

        public string UrlTraVe { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email là bắt buộc")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string MatKhau { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string XacNhanMatKhau { get; set; }
        }

        public async Task OnGetAsync(string urlTraVe = null)
        {
            UrlTraVe = urlTraVe;
        }

        public async Task<IActionResult> OnPostAsync(string urlTraVe = null)
        {
            urlTraVe ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var nguoiDung = CreateUser();

                await _userStore.SetUserNameAsync(nguoiDung, ThongTinDauVao.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(nguoiDung, ThongTinDauVao.Email, CancellationToken.None);
                var ketQua = await _userManager.CreateAsync(nguoiDung, ThongTinDauVao.MatKhau);

                if (ketQua.Succeeded)
                {
                    _logger.LogInformation("Tài khoản đã được tạo thành công cho email: {Email}", ThongTinDauVao.Email);

                    await _signInManager.SignInAsync(nguoiDung, isPersistent: false);
                    return LocalRedirect(urlTraVe);
                }
                foreach (var loi in ketQua.Errors)
                {
                    if (loi.Code == "DuplicateUserName" || loi.Code == "DuplicateEmail")
                    {
                        _logger.LogWarning("Đã có người dùng tạo tài khoản với email này rồi: {Email}", ThongTinDauVao.Email);
                        ModelState.AddModelError(string.Empty, "Email này đã được sử dụng. Vui lòng chọn email khác.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, loi.Description);
                    }
                }
            }

            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
