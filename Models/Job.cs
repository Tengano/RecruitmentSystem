using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Models
{
    public class Job
    {
        [Key]
        [Display(Name = "Mã công việc")]
        public int MaCongViec { get; set; }

        [Required(ErrorMessage = "Tiêu đề công việc là bắt buộc")]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả công việc là bắt buộc")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu công việc là bắt buộc")]
        [Display(Name = "Yêu cầu")]
        public string YeuCau { get; set; } = string.Empty;

        [Display(Name = "Phúc lợi")]
        public string PhucLoi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vị trí làm việc là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Địa điểm")]
        public string DiaDiem { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại công việc là bắt buộc")]
        [StringLength(50)]
        [Display(Name = "Loại công việc")]
        public string LoaiCongViec { get; set; } = string.Empty; // Toàn thời gian, Bán thời gian, Hợp đồng

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        [Display(Name = "Lương tối thiểu")]
        public decimal? LuongToiThieu { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Mức lương phải lớn hơn 0")]
        [Display(Name = "Lương tối đa")]
        public decimal? LuongToiDa { get; set; }

        [StringLength(100)]
        [Display(Name = "Công ty")]
        public string CongTy { get; set; } = string.Empty;

        [Display(Name = "Ngày đăng")]
        public DateTime NgayDang { get; set; } = DateTime.Now;

        [Display(Name = "Ngày kết thúc")]
        public DateTime? NgayKetThuc { get; set; }

        [Display(Name = "Hoạt động")]
        public bool HoatDong { get; set; } = true;

        [Display(Name = "Lượt xem")]
        public int LuotXem { get; set; } = 0;

        [StringLength(100)]
        [Display(Name = "Danh mục")]
        public string DanhMuc { get; set; } = string.Empty;

        [Display(Name = "Năm kinh nghiệm")]
        public int NamKinhNghiem { get; set; } = 0;

        // Navigation property
        public virtual ICollection<Application> DonUngTuyen { get; set; } = new List<Application>();
    }
}