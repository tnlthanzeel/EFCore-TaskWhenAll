using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class domainbase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Transactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "GemExport",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "GemExport",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "GemExport",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GemExport",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Exports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "Exports",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "Exports",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exports",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "GemExport");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "GemExport");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "GemExport");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GemExport");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Exports");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "Exports");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Exports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exports");
        }
    }
}
