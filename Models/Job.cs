using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class Job
    {
        [Key]
        [Display(Name = "Mã công việc")]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Tiêu đề công việc là bắt buộc")]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả công việc là bắt buộc")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu công việc là bắt buộc")]
        [Display(Name = "Yêu cầu")]
        public string Requirements { get; set; } = string.Empty;

        [Display(Name = "Phúc lợi")]
        public string Benefits { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vị trí làm việc là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Địa điểm")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại công việc là bắt buộc")]
        [StringLength(50)]
        [Display(Name = "Loại công việc")]
        public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        [Display(Name = "Lương tối thiểu")]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        [Display(Name = "Lương tối đa")]
        public decimal? SalaryMax { get; set; }

        [StringLength(100)]
        [Display(Name = "Công ty")]
        public string Company { get; set; } = string.Empty;

        [Display(Name = "Ngày đăng")]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Display(Name = "Ngày kết thúc")]
        public DateTime? ClosingDate { get; set; }

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Lượt xem")]
        public int Views { get; set; } = 0;

        [StringLength(100)]
        [Display(Name = "Danh mục")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Năm kinh nghiệm")]
        public int ExperienceYears { get; set; } = 0;

        // Navigation property
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

