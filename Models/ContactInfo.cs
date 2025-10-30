using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSystem.Models
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [StringLength(100)]
        [Display(Name = "Thành phố")]
        public string? ThanhPho { get; set; }

        [StringLength(100)]
        [Display(Name = "Quốc gia")]
        public string? QuocGia { get; set; }

        [StringLength(200)]
        [Display(Name = "LinkedIn")]
        public string? LinkedIn { get; set; }

        [StringLength(200)]
        [Display(Name = "GitHub")]
        public string? GitHub { get; set; }

        [StringLength(200)]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        public virtual User? User { get; set; }
    }
}

