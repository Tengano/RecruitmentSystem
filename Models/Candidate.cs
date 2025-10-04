using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

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

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string ResumeUrl { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}

