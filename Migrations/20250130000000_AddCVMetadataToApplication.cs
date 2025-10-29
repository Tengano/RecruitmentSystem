using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCVMetadataToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenFileCV",
                table: "Applications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DuongDanCV",
                table: "Applications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "KichThuocFile",
                table: "Applications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoaiFile",
                table: "Applications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayUploadCV",
                table: "Applications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenFileCV",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "DuongDanCV",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "KichThuocFile",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "LoaiFile",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "NgayUploadCV",
                table: "Applications");
        }
    }
}

