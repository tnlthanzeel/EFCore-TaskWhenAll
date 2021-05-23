using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedvariettytogemcertifiation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "Certifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_VarietyId",
                table: "Certifications",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Varieties_VarietyId",
                table: "Certifications",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Varieties_VarietyId",
                table: "Certifications");

            migrationBuilder.DropIndex(
                name: "IX_Certifications_VarietyId",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "Certifications");
        }
    }
}
