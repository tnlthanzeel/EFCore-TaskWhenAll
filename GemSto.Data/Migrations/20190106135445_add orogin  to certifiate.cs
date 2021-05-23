using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addorogintocertifiate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "Certificates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_OriginId",
                table: "Certificates",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Origins_OriginId",
                table: "Certificates",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Origins_OriginId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_OriginId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Certificates");
        }
    }
}
