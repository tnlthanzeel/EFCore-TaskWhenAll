using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class domainbaseaddedtobuyermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Buyers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "Buyers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "Buyers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Buyers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Buyers");
        }
    }
}
