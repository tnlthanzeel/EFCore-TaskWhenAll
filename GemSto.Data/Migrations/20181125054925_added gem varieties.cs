using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedgemvarieties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VarietyId",
                table: "Gems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Varieties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Varieties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gems_VarietyId",
                table: "Gems",
                column: "VarietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Varieties_VarietyId",
                table: "Gems",
                column: "VarietyId",
                principalTable: "Varieties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Varieties_VarietyId",
                table: "Gems");

            migrationBuilder.DropTable(
                name: "Varieties");

            migrationBuilder.DropIndex(
                name: "IX_Gems_VarietyId",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "VarietyId",
                table: "Gems");
        }
    }
}
