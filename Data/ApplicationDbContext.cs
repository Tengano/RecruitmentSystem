using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Models;

namespace RecruitmentSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Candidate> Candidates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Job entity
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.JobId);
                entity.Property(e => e.SalaryMin).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalaryMax).HasColumnType("decimal(18,2)");
            });

            // Configure Application entity
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);
                entity.HasOne(e => e.Job)
                    .WithMany(j => j.Applications)
                    .HasForeignKey(e => e.JobId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Candidate entity
            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.HasKey(e => e.CandidateId);
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
