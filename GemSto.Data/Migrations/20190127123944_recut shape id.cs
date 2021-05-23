using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class recutshapeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shapes_RescutId",
                table: "Gems");

            migrationBuilder.RenameColumn(
                name: "RescutId",
                table: "Gems",
                newName: "RecutShapeId");

            migrationBuilder.RenameIndex(
                name: "IX_Gems_RescutId",
                table: "Gems",
                newName: "IX_Gems_RecutShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shapes_RecutShapeId",
                table: "Gems",
                column: "RecutShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shapes_RecutShapeId",
                table: "Gems");

            migrationBuilder.RenameColumn(
                name: "RecutShapeId",
                table: "Gems",
                newName: "RescutId");

            migrationBuilder.RenameIndex(
                name: "IX_Gems_RecutShapeId",
                table: "Gems",
                newName: "IX_Gems_RescutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shapes_RescutId",
                table: "Gems",
                column: "RescutId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
