using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedthirdpartycolumntoge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsThirdParty",
                table: "Gems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsThirdParty",
                table: "Gems");
        }
    }
}
