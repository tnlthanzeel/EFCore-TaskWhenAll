using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class colourtableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColourId",
                table: "Certificate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Colour",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colour", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_ColourId",
                table: "Certificate",
                column: "ColourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Colour_ColourId",
                table: "Certificate",
                column: "ColourId",
                principalTable: "Colour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Colour_ColourId",
                table: "Certificate");

            migrationBuilder.DropTable(
                name: "Colour");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_ColourId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "ColourId",
                table: "Certificate");
        }
    }
}
