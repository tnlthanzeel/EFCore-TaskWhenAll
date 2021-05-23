using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class uniquekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StockNumber",
                table: "Gems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gems_StockNumber",
                table: "Gems",
                column: "StockNumber",
                unique: true,
                filter: "[StockNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gems_StockNumber",
                table: "Gems");

            migrationBuilder.AlterColumn<string>(
                name: "StockNumber",
                table: "Gems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
