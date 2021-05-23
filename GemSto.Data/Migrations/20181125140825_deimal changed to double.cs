using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class deimalchangedtodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PaidAmount",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<double>(
                name: "TotalAmountPaidToSeller",
                table: "Gems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmountPaidToSeller",
                table: "Gems");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
