using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedsellingpriceanddescriptionforgemApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Gems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GemApprovals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GemApprovals");
        }
    }
}
