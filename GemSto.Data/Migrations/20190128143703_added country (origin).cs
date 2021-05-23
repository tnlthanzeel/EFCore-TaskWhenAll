using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedcountryorigin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "Exports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exports_OriginId",
                table: "Exports",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exports_Origins_OriginId",
                table: "Exports",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exports_Origins_OriginId",
                table: "Exports");

            migrationBuilder.DropIndex(
                name: "IX_Exports_OriginId",
                table: "Exports");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exports");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Exports");
        }
    }
}
