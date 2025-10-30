using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class XoaBangCandidateVaThemUserIdVaoApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa bảng UngVien (Candidate) nếu tồn tại
            migrationBuilder.DropTable(
                name: "UngVien",
                schema: null);

            // Thêm cột UserId vào bảng DonUngTuyen (Application) nếu chưa có
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DonUngTuyen",
                type: "int",
                nullable: true);

            // Tạo index cho UserId
            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyen_UserId",
                table: "DonUngTuyen",
                column: "UserId");

            // Tạo foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_DonUngTuyen_NguoiDung_UserId",
                table: "DonUngTuyen",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_DonUngTuyen_NguoiDung_UserId",
                table: "DonUngTuyen");

            // Xóa index
            migrationBuilder.DropIndex(
                name: "IX_DonUngTuyen_UserId",
                table: "DonUngTuyen");

            // Xóa cột UserId
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DonUngTuyen");

            // Tạo lại bảng UngVien (không khuyến khích rollback)
            // Để trống vì không muốn khôi phục bảng cũ
        }
    }
}
