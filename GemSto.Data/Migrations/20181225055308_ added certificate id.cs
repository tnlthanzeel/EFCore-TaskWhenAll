using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedcertificateid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateFee",
                table: "Certificates");

            migrationBuilder.AddColumn<int>(
                name: "CertificateId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CertificateId",
                table: "Transactions",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Certificates_CertificateId",
                table: "Transactions",
                column: "CertificateId",
                principalTable: "Certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Certificates_CertificateId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CertificateId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Transactions");

            migrationBuilder.AddColumn<double>(
                name: "CertificateFee",
                table: "Certificates",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
