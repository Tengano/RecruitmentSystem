using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Tiêu đề công việc là bắt buộc")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả công việc là bắt buộc")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu công việc là bắt buộc")]
        public string Requirements { get; set; } = string.Empty;

        public string Benefits { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vị trí làm việc là bắt buộc")]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại công việc là bắt buộc")]
        [StringLength(50)]
        public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        public decimal? SalaryMax { get; set; }

        [StringLength(100)]
        public string Company { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int Views { get; set; } = 0;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        public int ExperienceYears { get; set; } = 0;

        // Navigation property
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

