using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Trường/Tổ chức")]
        public string TenTruong { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Bằng cấp")]
        public string BangCap { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Chuyên ngành")]
        public string? ChuyenNganh { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? NgayKetThuc { get; set; }

        [Display(Name = "Đang học")]
        public bool DangHoc { get; set; } = false;

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        [Display(Name = "Điểm GPA")]
        public decimal? DiemGPA { get; set; }

        public virtual User? User { get; set; }
    }
}