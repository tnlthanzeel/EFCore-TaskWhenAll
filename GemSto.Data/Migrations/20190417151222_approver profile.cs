using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class approverprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalNumber",
                table: "Approvals");

            migrationBuilder.DropColumn(
                name: "ApproverName",
                table: "Approvals");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Approvals");

            migrationBuilder.DropColumn(
                name: "IsApprovalClosed",
                table: "Approvals");

            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "Approvals",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Approvals_ApproverId",
                table: "Approvals",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Approvers_ApproverId",
                table: "Approvals",
                column: "ApproverId",
                principalTable: "Approvers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Approvers_ApproverId",
                table: "Approvals");

            migrationBuilder.DropIndex(
                name: "IX_Approvals_ApproverId",
                table: "Approvals");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "Approvals");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalNumber",
                table: "Approvals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApproverName",
                table: "Approvals",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Date",
                table: "Approvals",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovalClosed",
                table: "Approvals",
                nullable: false,
                defaultValue: false);
        }
    }
}
