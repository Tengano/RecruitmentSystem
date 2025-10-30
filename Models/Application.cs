using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class Application
    {
        [Key]
        [Display(Name = "Mã đơn ứng tuyển")]
        public int MaDonUngTuyen { get; set; }

        [Required]
        [Display(Name = "Mã công việc")]
        public int MaCongViec { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = string.Empty;

        [Display(Name = "Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Trình độ học vấn")]
        public string TrinhDoHocVan { get; set; } = string.Empty;

        [Display(Name = "Kinh nghiệm")]
        public string KinhNghiem { get; set; } = string.Empty;

        [Display(Name = "Kỹ năng")]
        public string KyNang { get; set; } = string.Empty;

        [Display(Name = "Ngày ứng tuyển")]
        public DateTime NgayUngTuyen { get; set; } = DateTime.Now;

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Chờ xem xét";

        [ForeignKey("MaCongViec")]
        public virtual Job? CongViec { get; set; }
    }
    
}