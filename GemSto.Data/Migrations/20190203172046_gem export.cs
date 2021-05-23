using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class gemexport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificateProviderId",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColourId",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "GemExports",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsTreated",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "GemExports",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "GemExports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GemExports_CertificateProviderId",
                table: "GemExports",
                column: "CertificateProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_GemExports_ColourId",
                table: "GemExports",
                column: "ColourId");

            migrationBuilder.CreateIndex(
                name: "IX_GemExports_ShapeId",
                table: "GemExports",
                column: "ShapeId");

            migrationBuilder.CreateIndex(
                name: "IX_GemExports_VarietyId",
                table: "GemExports",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_CertificateProviders_CertificateProviderId",
                table: "GemExports",
                column: "CertificateProviderId",
                principalTable: "CertificateProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Colours_ColourId",
                table: "GemExports",
                column: "ColourId",
                principalTable: "Colours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Shapes_ShapeId",
                table: "GemExports",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Varieties_VarietyId",
                table: "GemExports",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_CertificateProviders_CertificateProviderId",
                table: "GemExports");

            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Colours_ColourId",
                table: "GemExports");

            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Shapes_ShapeId",
                table: "GemExports");

            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Varieties_VarietyId",
                table: "GemExports");

            migrationBuilder.DropIndex(
                name: "IX_GemExports_CertificateProviderId",
                table: "GemExports");

            migrationBuilder.DropIndex(
                name: "IX_GemExports_ColourId",
                table: "GemExports");

            migrationBuilder.DropIndex(
                name: "IX_GemExports_ShapeId",
                table: "GemExports");

            migrationBuilder.DropIndex(
                name: "IX_GemExports_VarietyId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "CertificateProviderId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "ColourId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "IsTreated",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "GemExports");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "GemExports");
        }
    }
}
