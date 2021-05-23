using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class originidaddedtogemexport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "GemExports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemExports_OriginId",
                table: "GemExports",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Origins_OriginId",
                table: "GemExports",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Origins_OriginId",
                table: "GemExports");

            migrationBuilder.DropIndex(
                name: "IX_GemExports_OriginId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "GemExports");
        }
    }
}
