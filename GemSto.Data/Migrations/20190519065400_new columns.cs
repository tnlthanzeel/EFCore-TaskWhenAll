using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class newcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTreated",
                table: "GemSales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdPartyOwner",
                table: "GemSales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTreated",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "ThirdPartyOwner",
                table: "GemSales");
        }
    }
}
