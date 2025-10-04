using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        public string CoverLetter { get; set; } = string.Empty;

        [StringLength(200)]
        public string ResumeUrl { get; set; } = string.Empty;

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Interview, Rejected, Accepted

        public string Notes { get; set; } = string.Empty;

        // Navigation property
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }
    }
}

