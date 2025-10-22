using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CongViec",
                columns: table => new
                {
                    MaCongViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhucLoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoaiCongViec = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LuongToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LuongToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CongTy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoatDong = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LuotXem = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NamKinhNghiem = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongViec", x => x.MaCongViec);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "User"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    HoatDong = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UngVien",
                columns: table => new
                {
                    MaUngVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TrinhDoHocVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KyNang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UngVien", x => x.MaUngVien);
                });

            migrationBuilder.CreateTable(
                name: "DonUngTuyen",
                columns: table => new
                {
                    MaDonUngTuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCongViec = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TrinhDoHocVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KyNang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Chờ xem xét")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonUngTuyen", x => x.MaDonUngTuyen);
                    table.ForeignKey(
                        name: "FK_DonUngTuyen_CongViec_MaCongViec",
                        column: x => x.MaCongViec,
                        principalTable: "CongViec",
                        principalColumn: "MaCongViec",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CongViec_DanhMuc",
                table: "CongViec",
                column: "DanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_CongViec_DiaDiem",
                table: "CongViec",
                column: "DiaDiem");

            migrationBuilder.CreateIndex(
                name: "IX_CongViec_HoatDong",
                table: "CongViec",
                column: "HoatDong");

            migrationBuilder.CreateIndex(
                name: "IX_CongViec_LoaiCongViec",
                table: "CongViec",
                column: "LoaiCongViec");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyen_MaCongViec",
                table: "DonUngTuyen",
                column: "MaCongViec");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyen_NgayUngTuyen",
                table: "DonUngTuyen",
                column: "NgayUngTuyen");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyen_TrangThai",
                table: "DonUngTuyen",
                column: "TrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Email",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UngVien_Email",
                table: "UngVien",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_UngVien_NgayTao",
                table: "UngVien",
                column: "NgayTao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonUngTuyen");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "UngVien");

            migrationBuilder.DropTable(
                name: "CongViec");
        }
    }
}
