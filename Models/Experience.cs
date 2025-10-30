using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Experience
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên công ty")]
        public string TenCongTy { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Vị trí")]
        public string ViTri { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Địa điểm")]
        public string? DiaDiem { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? NgayKetThuc { get; set; }

        [Display(Name = "Đang làm việc")]
        public bool DangLamViec { get; set; } = false;

        [StringLength(2000)]
        [Display(Name = "Mô tả công việc")]
        public string? MoTaCongViec { get; set; }

        [StringLength(500)]
        [Display(Name = "Thành tích")]
        public string? ThanhTich { get; set; }

        public virtual User? User { get; set; }
    }
}

