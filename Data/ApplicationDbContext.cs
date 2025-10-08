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
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho bảng User -> NguoiDung
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

                // Tạo index
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Cấu hình cho bảng Job -> CongViec
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("CongViec");
                entity.HasKey(e => e.MaCongViec);
                entity.Property(e => e.TieuDe).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MoTa).IsRequired();
                entity.Property(e => e.YeuCau).IsRequired();
                entity.Property(e => e.DiaDiem).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LoaiCongViec).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CongTy).HasMaxLength(100);
                entity.Property(e => e.DanhMuc).HasMaxLength(100);
                entity.Property(e => e.NgayDang).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.HoatDong).HasDefaultValue(true);
                entity.Property(e => e.LuotXem).HasDefaultValue(0);
                entity.Property(e => e.NamKinhNghiem).HasDefaultValue(0);

                // Cấu hình decimal
                entity.Property(e => e.LuongToiThieu).HasColumnType("decimal(18,2)");
                entity.Property(e => e.LuongToiDa).HasColumnType("decimal(18,2)");

                // Tạo index
                entity.HasIndex(e => e.DiaDiem);
                entity.HasIndex(e => e.LoaiCongViec);
                entity.HasIndex(e => e.DanhMuc);
                entity.HasIndex(e => e.HoatDong);
            });

            // Cấu hình cho bảng Candidate -> UngVien
            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("UngVien");
                entity.HasKey(e => e.MaUngVien);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).HasMaxLength(20);
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.TrinhDoHocVan).HasMaxLength(50);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");

                // Tạo index
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.NgayTao);
            });

            // Cấu hình cho bảng Application -> DonUngTuyen
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("DonUngTuyen");
                entity.HasKey(e => e.MaDonUngTuyen);
                entity.Property(e => e.MaCongViec).IsRequired();
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).HasMaxLength(20);
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.TrinhDoHocVan).HasMaxLength(50);
                entity.Property(e => e.TrangThai).IsRequired().HasMaxLength(50).HasDefaultValue("Chờ xem xét");
                entity.Property(e => e.NgayUngTuyen).HasDefaultValueSql("GETDATE()");

                // Tạo foreign key
                entity.HasOne(e => e.CongViec)
                      .WithMany(e => e.DonUngTuyen)
                      .HasForeignKey(e => e.MaCongViec)
                      .OnDelete(DeleteBehavior.Cascade);

                // Tạo index
                entity.HasIndex(e => e.MaCongViec);
                entity.HasIndex(e => e.TrangThai);
                entity.HasIndex(e => e.NgayUngTuyen);
            });

            // Cấu hình cho bảng Contact -> LienHe
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("LienHe");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).IsRequired().HasMaxLength(20);
                entity.Property(e => e.NoiDung).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.NgayGui).HasDefaultValueSql("GETDATE()");

                // Tạo index
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.NgayGui);
            });
        }
    }
}
