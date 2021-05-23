using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedapprovalforgemsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GemApprovalId",
                table: "GemSales",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_GemApprovalId",
                table: "GemSales",
                column: "GemApprovalId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_GemApprovals_GemApprovalId",
                table: "GemSales",
                column: "GemApprovalId",
                principalTable: "GemApprovals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_GemApprovals_GemApprovalId",
                table: "GemSales");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_GemApprovalId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "GemApprovalId",
                table: "GemSales");
        }
    }
}
