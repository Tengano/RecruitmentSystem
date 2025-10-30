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
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

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

                entity.Property(e => e.LuongToiThieu).HasColumnType("decimal(18,2)");
                entity.Property(e => e.LuongToiDa).HasColumnType("decimal(18,2)");

                entity.HasIndex(e => e.DiaDiem);
                entity.HasIndex(e => e.LoaiCongViec);
                entity.HasIndex(e => e.DanhMuc);
                entity.HasIndex(e => e.HoatDong);
            });

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

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CongViec)
                      .WithMany(e => e.DonUngTuyen)
                      .HasForeignKey(e => e.MaCongViec)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.MaCongViec);
                entity.HasIndex(e => e.TrangThai);
                entity.HasIndex(e => e.NgayUngTuyen);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("LienHe");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SoDienThoai).IsRequired().HasMaxLength(20);
                entity.Property(e => e.NoiDung).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.NgayGui).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.NgayGui);
            });

            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ThongTinLienHe");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SoDienThoai).HasMaxLength(20);
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.ThanhPho).HasMaxLength(100);
                entity.Property(e => e.QuocGia).HasMaxLength(100);
                entity.Property(e => e.LinkedIn).HasMaxLength(200);
                entity.Property(e => e.GitHub).HasMaxLength(200);
                entity.Property(e => e.Website).HasMaxLength(200);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.ToTable("HocVan");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TenTruong).IsRequired().HasMaxLength(200);
                entity.Property(e => e.BangCap).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ChuyenNganh).HasMaxLength(100);
                entity.Property(e => e.MoTa).HasMaxLength(1000);
                entity.Property(e => e.DiemGPA).HasColumnType("decimal(3,2)");
                entity.Property(e => e.DangHoc).HasDefaultValue(false);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.NgayBatDau);
            });

            modelBuilder.Entity<Experience>(entity =>
            {
                entity.ToTable("KinhNghiem");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TenCongTy).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ViTri).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DiaDiem).HasMaxLength(100);
                entity.Property(e => e.MoTaCongViec).HasMaxLength(2000);
                entity.Property(e => e.ThanhTich).HasMaxLength(500);
                entity.Property(e => e.DangLamViec).HasDefaultValue(false);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.NgayBatDau);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("KyNang");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TenKyNang).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CapDo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DanhMuc).HasMaxLength(100);
                entity.Property(e => e.PhanTramThanhThao).HasDefaultValue(50);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.DanhMuc);
            });
        }
    }
}
