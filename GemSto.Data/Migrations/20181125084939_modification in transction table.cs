using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class modificationintransctiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerPaymentStatus",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "PaidToSellerOn",
                table: "Transaction",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "AmountPaidToSeller",
                table: "Transaction",
                newName: "PaidAmount");

            migrationBuilder.AlterColumn<int>(
                name: "GemStatus",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaidOn",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Transaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidOn",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "Transaction",
                newName: "AmountPaidToSeller");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Transaction",
                newName: "PaidToSellerOn");

            migrationBuilder.AlterColumn<int>(
                name: "GemStatus",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellerPaymentStatus",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);
        }
    }
}
