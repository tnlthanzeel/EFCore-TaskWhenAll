using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class updatetogemsalestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "GemSales",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "GemSales",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_ShapeId",
                table: "GemSales",
                column: "ShapeId");

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_VarietyId",
                table: "GemSales",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Shapes_ShapeId",
                table: "GemSales",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Varieties_VarietyId",
                table: "GemSales",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Shapes_ShapeId",
                table: "GemSales");

            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Varieties_VarietyId",
                table: "GemSales");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_ShapeId",
                table: "GemSales");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_VarietyId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "GemSales");
        }
    }
}
