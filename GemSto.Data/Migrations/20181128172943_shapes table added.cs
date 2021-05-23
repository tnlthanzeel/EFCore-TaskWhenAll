using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class shapestableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shape_ShapeId",
                table: "Gems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shape",
                table: "Shape");

            migrationBuilder.RenameTable(
                name: "Shape",
                newName: "Shapes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shapes_ShapeId",
                table: "Gems",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shapes_ShapeId",
                table: "Gems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes");

            migrationBuilder.RenameTable(
                name: "Shapes",
                newName: "Shape");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shape",
                table: "Shape",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shape_ShapeId",
                table: "Gems",
                column: "ShapeId",
                principalTable: "Shape",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
