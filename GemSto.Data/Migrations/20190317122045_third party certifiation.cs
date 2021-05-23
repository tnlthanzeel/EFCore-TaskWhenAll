using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class thirdpartycertifiation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Gems_GemId",
                table: "Certifications");

            migrationBuilder.AlterColumn<int>(
                name: "GemId",
                table: "Certifications",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsThirdParty",
                table: "Certifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Certifications",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Certifications",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Gems_GemId",
                table: "Certifications",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Gems_GemId",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "IsThirdParty",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Certifications");

            migrationBuilder.AlterColumn<int>(
                name: "GemId",
                table: "Certifications",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Gems_GemId",
                table: "Certifications",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
