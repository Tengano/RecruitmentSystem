using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Application
    {
        [Key]
        [Display(Name = "Mã đơn ứng tuyển")]
        public int ApplicationId { get; set; }

        [Required]
        [Display(Name = "Mã công việc")]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Trình độ học vấn")]
        public string Education { get; set; } = string.Empty;

        [Display(Name = "Kinh nghiệm")]
        public string Experience { get; set; } = string.Empty;

        [Display(Name = "Kỹ năng")]
        public string Skills { get; set; } = string.Empty;

        [Display(Name = "Ngày ứng tuyển")]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ xem xét"; // Chờ xem xét, Đã phỏng vấn, Đã từ chối

        // Navigation property
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }
    }
}

