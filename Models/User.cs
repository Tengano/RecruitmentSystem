using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Mã người dùng")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [StringLength(20, ErrorMessage = "Vai trò không được vượt quá 20 ký tự")]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } = "User"; // Admin, User
        
        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        [Display(Name = "Hoạt động")]
        public bool HoatDong { get; set; } = true;
    }
}
