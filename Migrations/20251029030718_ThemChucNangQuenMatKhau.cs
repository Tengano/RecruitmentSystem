using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class ThemChucNangQuenMatKhau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaDatLaiMatKhau",
                table: "NguoiDung",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianHetHanMa",
                table: "NguoiDung",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LienHe_Email",
                table: "LienHe",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_LienHe_NgayGui",
                table: "LienHe",
                column: "NgayGui");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropColumn(
                name: "MaDatLaiMatKhau",
                table: "NguoiDung");

            migrationBuilder.DropColumn(
                name: "ThoiGianHetHanMa",
                table: "NguoiDung");
        }
    }
}
