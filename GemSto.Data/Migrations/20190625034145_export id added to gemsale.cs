using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class exportidaddedtogemsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GemExportId",
                table: "GemSales",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_GemExportId",
                table: "GemSales",
                column: "GemExportId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_GemExports_GemExportId",
                table: "GemSales",
                column: "GemExportId",
                principalTable: "GemExports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_GemExports_GemExportId",
                table: "GemSales");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_GemExportId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "GemExportId",
                table: "GemSales");
        }
    }
}
