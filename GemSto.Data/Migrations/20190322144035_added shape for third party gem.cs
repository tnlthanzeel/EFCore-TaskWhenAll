using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedshapeforthirdpartygem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "Certifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_ShapeId",
                table: "Certifications",
                column: "ShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Shapes_ShapeId",
                table: "Certifications",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Shapes_ShapeId",
                table: "Certifications");

            migrationBuilder.DropIndex(
                name: "IX_Certifications_ShapeId",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "Certifications");
        }
    }
}
