using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedsellertogems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Gems_SellerId",
                table: "Gems",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Sellers_SellerId",
                table: "Gems",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Sellers_SellerId",
                table: "Gems");

            migrationBuilder.DropIndex(
                name: "IX_Gems_SellerId",
                table: "Gems");
        }
    }
}
