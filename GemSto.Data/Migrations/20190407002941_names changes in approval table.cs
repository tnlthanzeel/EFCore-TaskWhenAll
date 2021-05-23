using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class nameschangesinapprovaltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoOfPieces",
                table: "Approvals",
                newName: "NumberOfPieces");

            migrationBuilder.RenameColumn(
                name: "Approver",
                table: "Approvals",
                newName: "ApproverName");

            migrationBuilder.RenameColumn(
                name: "ApprovalDate",
                table: "Approvals",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfPieces",
                table: "Approvals",
                newName: "NoOfPieces");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Approvals",
                newName: "ApprovalDate");

            migrationBuilder.RenameColumn(
                name: "ApproverName",
                table: "Approvals",
                newName: "Approver");
        }
    }
}
