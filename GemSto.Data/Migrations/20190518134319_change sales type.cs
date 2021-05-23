using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class changesalestype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Gems_GemId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "IsSingleSale",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SellingRate",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalAmountPaid",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GemSales",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCertificatePending",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "GemId",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsSingleSale",
                table: "GemSales",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThirdParty",
                table: "GemSales",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "GemSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingRate",
                table: "GemSales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountPaid",
                table: "GemSales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Gems_GemId",
                table: "GemSales",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Gems_GemId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "IsSingleSale",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "IsThirdParty",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "SellingRate",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "TotalAmountPaid",
                table: "GemSales");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "GemSales",
                newName: "Amount");

            migrationBuilder.AddColumn<bool>(
                name: "IsSingleSale",
                table: "Sales",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Sales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingRate",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountPaid",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCertificatePending",
                table: "GemSales",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GemId",
                table: "GemSales",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Gems_GemId",
                table: "GemSales",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
