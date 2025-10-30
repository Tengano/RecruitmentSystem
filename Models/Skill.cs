using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên kỹ năng")]
        public string TenKyNang { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Cấp độ")]
        public string CapDo { get; set; } = "Trung bình"; // Cơ bản, Trung bình, Khá, Giỏi, Xuất sắc

        [Range(0, 100)]
        [Display(Name = "Phần trăm thành thạo")]
        public int PhanTramThanhThao { get; set; } = 50;

        [StringLength(100)]
        [Display(Name = "Danh mục")]
        public string? DanhMuc { get; set; } // Ngôn ngữ lập trình, Framework, Tool, Soft skill, etc.

        [Display(Name = "Số năm kinh nghiệm")]
        public int? SoNamKinhNghiem { get; set; }

        public virtual User? User { get; set; }
    }
}

