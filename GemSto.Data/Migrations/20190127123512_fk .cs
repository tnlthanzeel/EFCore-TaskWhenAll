using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RescutId",
                table: "Gems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gems_RescutId",
                table: "Gems",
                column: "RescutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shapes_RescutId",
                table: "Gems",
                column: "RescutId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shapes_RescutId",
                table: "Gems");

            migrationBuilder.DropIndex(
                name: "IX_Gems_RescutId",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "RescutId",
                table: "Gems");
        }
    }
}
