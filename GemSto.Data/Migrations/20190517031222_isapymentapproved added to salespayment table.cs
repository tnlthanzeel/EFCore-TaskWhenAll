using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class isapymentapprovedaddedtosalespaymenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentApproved",
                table: "SalePayments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaymentApproved",
                table: "SalePayments");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "GemSales",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
