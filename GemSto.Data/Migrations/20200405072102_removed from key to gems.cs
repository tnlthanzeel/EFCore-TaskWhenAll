using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class removedfromkeytogems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemHistory_Gems_GemId",
                table: "GemHistory");

            migrationBuilder.DropIndex(
                name: "IX_GemHistory_GemId",
                table: "GemHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GemHistory_GemId",
                table: "GemHistory",
                column: "GemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemHistory_Gems_GemId",
                table: "GemHistory",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
