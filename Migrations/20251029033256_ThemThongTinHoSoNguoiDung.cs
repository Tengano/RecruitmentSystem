using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class ThemThongTinHoSoNguoiDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HocVan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TenTruong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BangCap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChuyenNganh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DangHoc = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DiemGPA = table.Column<decimal>(type: "decimal(3,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocVan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HocVan_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KinhNghiem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TenCongTy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ViTri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DangLamViec = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MoTaCongViec = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ThanhTich = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KinhNghiem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KinhNghiem_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KyNang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TenKyNang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CapDo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhanTramThanhThao = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
                    DanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoNamKinhNghiem = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyNang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KyNang_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinLienHe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GitHub = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinLienHe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinLienHe_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HocVan_NgayBatDau",
                table: "HocVan",
                column: "NgayBatDau");

            migrationBuilder.CreateIndex(
                name: "IX_HocVan_UserId",
                table: "HocVan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KinhNghiem_NgayBatDau",
                table: "KinhNghiem",
                column: "NgayBatDau");

            migrationBuilder.CreateIndex(
                name: "IX_KinhNghiem_UserId",
                table: "KinhNghiem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KyNang_DanhMuc",
                table: "KyNang",
                column: "DanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_KyNang_UserId",
                table: "KyNang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinLienHe_UserId",
                table: "ThongTinLienHe",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HocVan");

            migrationBuilder.DropTable(
                name: "KinhNghiem");

            migrationBuilder.DropTable(
                name: "KyNang");

            migrationBuilder.DropTable(
                name: "ThongTinLienHe");
        }
    }
}
