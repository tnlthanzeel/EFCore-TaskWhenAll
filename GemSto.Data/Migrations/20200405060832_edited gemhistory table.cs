using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class editedgemhistorytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistoryStatusEnum",
                table: "GemHistory");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "GemHistory",
                newName: "CreatedByName");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "GemHistory",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "GemHistoryStatusEnum",
                table: "GemHistory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "GemHistory");

            migrationBuilder.DropColumn(
                name: "GemHistoryStatusEnum",
                table: "GemHistory");

            migrationBuilder.RenameColumn(
                name: "CreatedByName",
                table: "GemHistory",
                newName: "UserName");

            migrationBuilder.AddColumn<int>(
                name: "HistoryStatusEnum",
                table: "GemHistory",
                nullable: false,
                defaultValue: 0);
        }
    }
}
