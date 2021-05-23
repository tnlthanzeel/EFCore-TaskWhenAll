using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class weigth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThirdPartyCertificates_CertificationId",
                table: "ThirdPartyCertificates");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ThirdPartyCertificates",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_CertificationId",
                table: "ThirdPartyCertificates",
                column: "CertificationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThirdPartyCertificates_CertificationId",
                table: "ThirdPartyCertificates");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ThirdPartyCertificates",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_CertificationId",
                table: "ThirdPartyCertificates",
                column: "CertificationId");
        }
    }
}
