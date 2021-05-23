using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class salenumberuniqued : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SaleNumber",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_Number",
                table: "Sales",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_SaleNumber",
                table: "GemSales",
                column: "SaleNumber",
                unique: true,
                filter: "[SaleNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sales_Number",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_SaleNumber",
                table: "GemSales");

            migrationBuilder.AlterColumn<string>(
                name: "SaleNumber",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
