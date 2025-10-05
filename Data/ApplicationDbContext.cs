using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Models;

namespace RecruitmentSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Candidate> Candidates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("NguoiDung");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenDangNhap).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MatKhau).IsRequired().HasMaxLength(100);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.VaiTro).IsRequired().HasMaxLength(20).HasDefaultValue("User");
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.HoatDong).HasDefaultValue(true);
                
                // Tạo unique index
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Job entity
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("CongViec"); // Đổi tên bảng sang tiếng Việt
                entity.HasKey(e => e.JobId);
                entity.Property(e => e.SalaryMin).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalaryMax).HasColumnType("decimal(18,2)");
                
                // Đổi tên các cột sang tiếng Việt
                entity.Property(e => e.Title).HasColumnName("TieuDe");
                entity.Property(e => e.Description).HasColumnName("MoTa");
                entity.Property(e => e.Requirements).HasColumnName("YeuCau");
                entity.Property(e => e.Benefits).HasColumnName("PhucLoi");
                entity.Property(e => e.Location).HasColumnName("DiaDiem");
                entity.Property(e => e.JobType).HasColumnName("LoaiCongViec");
                entity.Property(e => e.SalaryMin).HasColumnName("LuongToiThieu");
                entity.Property(e => e.SalaryMax).HasColumnName("LuongToiDa");
                entity.Property(e => e.Company).HasColumnName("CongTy");
                entity.Property(e => e.PostedDate).HasColumnName("NgayDang");
                entity.Property(e => e.ClosingDate).HasColumnName("NgayKetThuc");
                entity.Property(e => e.IsActive).HasColumnName("HoatDong");
                entity.Property(e => e.Views).HasColumnName("LuotXem");
                entity.Property(e => e.Category).HasColumnName("DanhMuc");
                entity.Property(e => e.ExperienceYears).HasColumnName("NamKinhNghiem");
            });

            // Configure Application entity
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("DonUngTuyen");
                entity.HasKey(e => e.ApplicationId);
                entity.HasOne(e => e.Job)
                    .WithMany(j => j.Applications)
                    .HasForeignKey(e => e.JobId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Map to Vietnamese column names
                entity.Property(e => e.FullName).HasColumnName("HoTen");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Phone).HasColumnName("SoDienThoai");
                entity.Property(e => e.Address).HasColumnName("DiaChi");
                entity.Property(e => e.DateOfBirth).HasColumnName("NgaySinh");
                entity.Property(e => e.Gender).HasColumnName("GioiTinh");
                entity.Property(e => e.Education).HasColumnName("TrinhDoHocVan");
                entity.Property(e => e.Experience).HasColumnName("KinhNghiem");
                entity.Property(e => e.Skills).HasColumnName("KyNang");
                entity.Property(e => e.AppliedDate).HasColumnName("NgayUngTuyen");
                entity.Property(e => e.Status).HasColumnName("TrangThai");
            });

            // Configure Candidate entity
            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("UngVien");
                entity.HasKey(e => e.CandidateId);
                
                // Map to Vietnamese column names
                entity.Property(e => e.FullName).HasColumnName("HoTen");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Phone).HasColumnName("SoDienThoai");
                entity.Property(e => e.Address).HasColumnName("DiaChi");
                entity.Property(e => e.DateOfBirth).HasColumnName("NgaySinh");
                entity.Property(e => e.Gender).HasColumnName("GioiTinh");
                entity.Property(e => e.Education).HasColumnName("TrinhDoHocVan");
                entity.Property(e => e.Experience).HasColumnName("KinhNghiem");
                entity.Property(e => e.Skills).HasColumnName("KyNang");
                entity.Property(e => e.RegisteredDate).HasColumnName("NgayTao");
            });

            // Seed data
            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    JobId = 1,
                    Title = "Lập trình viên .NET",
                    Description = "Chúng tôi đang tìm kiếm lập trình viên .NET có kinh nghiệm để tham gia vào dự án phát triển ứng dụng web cho doanh nghiệp.",
                    Requirements = "- Kinh nghiệm 2+ năm với .NET Core/ASP.NET\n- Thành thạo C#, Entity Framework\n- Kinh nghiệm với SQL Server\n- Có kiến thức về HTML, CSS, JavaScript",
                    Benefits = "- Mức lương cạnh tranh\n- Thưởng theo dự án\n- Bảo hiểm đầy đủ\n- Môi trường làm việc chuyên nghiệp",
                    Location = "Hà Nội",
                    JobType = "Full-time",
                    SalaryMin = 15000000,
                    SalaryMax = 25000000,
                    Company = "Tech Solutions JSC",
                    PostedDate = DateTime.Now,
                    ClosingDate = DateTime.Now.AddMonths(1),
                    IsActive = true,
                    Category = "Công nghệ thông tin",
                    ExperienceYears = 2
                },
                new Job
                {
                    JobId = 2,
                    Title = "Frontend Developer (React)",
                    Description = "Tham gia phát triển giao diện người dùng cho các ứng dụng web hiện đại sử dụng React và các công nghệ frontend mới nhất.",
                    Requirements = "- Kinh nghiệm 1+ năm với React/Next.js\n- Thành thạo HTML, CSS, JavaScript/TypeScript\n- Hiểu biết về UI/UX design\n- Kinh nghiệm với RESTful APIs",
                    Benefits = "- Lương từ 12-20 triệu\n- Review lương 6 tháng/lần\n- Đào tạo và phát triển kỹ năng\n- Team building định kỳ",
                    Location = "Hồ Chí Minh",
                    JobType = "Full-time",
                    SalaryMin = 12000000,
                    SalaryMax = 20000000,
                    Company = "Digital Innovation Co.",
                    PostedDate = DateTime.Now,
                    ClosingDate = DateTime.Now.AddMonths(1),
                    IsActive = true,
                    Category = "Công nghệ thông tin",
                    ExperienceYears = 1
                },
                new Job
                {
                    JobId = 3,
                    Title = "Business Analyst",
                    Description = "Phân tích nghiệp vụ và yêu cầu khách hàng, làm cầu nối giữa khách hàng và đội ngũ phát triển.",
                    Requirements = "- Tốt nghiệp đại học chuyên ngành liên quan\n- Kinh nghiệm 1+ năm vị trí tương đương\n- Kỹ năng phân tích, tổng hợp tốt\n- Tiếng Anh giao tiếp tốt",
                    Benefits = "- Lương 10-15 triệu\n- Được đào tạo bài bản\n- Cơ hội thăng tiến\n- Môi trường quốc tế",
                    Location = "Đà Nẵng",
                    JobType = "Full-time",
                    SalaryMin = 10000000,
                    SalaryMax = 15000000,
                    Company = "Global Tech Vietnam",
                    PostedDate = DateTime.Now,
                    ClosingDate = DateTime.Now.AddMonths(1),
                    IsActive = true,
                    Category = "Kinh doanh",
                    ExperienceYears = 1
                }
            );
        }
    }
}
