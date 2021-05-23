using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Origins",
                newName: "Value");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Origins",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Origins");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Origins",
                newName: "Name");
        }
    }
}
