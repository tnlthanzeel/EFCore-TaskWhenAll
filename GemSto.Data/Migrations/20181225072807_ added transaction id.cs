using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedtransactionid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Certificates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_TransactionId",
                table: "Certificates",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Transactions_TransactionId",
                table: "Certificates",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Transactions_TransactionId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_TransactionId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "TransactionId",
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
    }
}
