using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedgemexpo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemExport_Exports_ExportId",
                table: "GemExport");

            migrationBuilder.DropForeignKey(
                name: "FK_GemExport_Gems_GemId",
                table: "GemExport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GemExport",
                table: "GemExport");

            migrationBuilder.RenameTable(
                name: "GemExport",
                newName: "GemExports");

            migrationBuilder.RenameIndex(
                name: "IX_GemExport_GemId",
                table: "GemExports",
                newName: "IX_GemExports_GemId");

            migrationBuilder.RenameIndex(
                name: "IX_GemExport_ExportId",
                table: "GemExports",
                newName: "IX_GemExports_ExportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GemExports",
                table: "GemExports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Exports_ExportId",
                table: "GemExports",
                column: "ExportId",
                principalTable: "Exports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GemExports_Gems_GemId",
                table: "GemExports",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Exports_ExportId",
                table: "GemExports");

            migrationBuilder.DropForeignKey(
                name: "FK_GemExports_Gems_GemId",
                table: "GemExports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GemExports",
                table: "GemExports");

            migrationBuilder.RenameTable(
                name: "GemExports",
                newName: "GemExport");

            migrationBuilder.RenameIndex(
                name: "IX_GemExports_GemId",
                table: "GemExport",
                newName: "IX_GemExport_GemId");

            migrationBuilder.RenameIndex(
                name: "IX_GemExports_ExportId",
                table: "GemExport",
                newName: "IX_GemExport_ExportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GemExport",
                table: "GemExport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GemExport_Exports_ExportId",
                table: "GemExport",
                column: "ExportId",
                principalTable: "Exports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GemExport_Gems_GemId",
                table: "GemExport",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
