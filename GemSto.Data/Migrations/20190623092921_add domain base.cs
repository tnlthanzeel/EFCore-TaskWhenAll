using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class adddomainbase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "MiscellaneousPayments",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "MiscellaneousPayments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "MiscellaneousPayments",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "MiscellaneousPayments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MiscellaneousPayments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Miscellaneous",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Miscellaneous",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "EditedById",
                table: "Miscellaneous",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditedOn",
                table: "Miscellaneous",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Miscellaneous",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "MiscellaneousPayments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MiscellaneousPayments");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "MiscellaneousPayments");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "MiscellaneousPayments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MiscellaneousPayments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Miscellaneous");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Miscellaneous");

            migrationBuilder.DropColumn(
                name: "EditedById",
                table: "Miscellaneous");

            migrationBuilder.DropColumn(
                name: "EditedOn",
                table: "Miscellaneous");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Miscellaneous");
        }
    }
}
