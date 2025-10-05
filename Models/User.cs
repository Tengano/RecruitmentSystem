using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string MatKhau { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string VaiTro { get; set; } = "User"; // Admin, User
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        public bool HoatDong { get; set; } = true;
    }
}
