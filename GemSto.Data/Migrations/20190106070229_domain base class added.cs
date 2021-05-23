using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class domainbaseclassadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Gems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "Gems",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "Gems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Gems");
        }
    }
}
