using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedgemlottable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GemLotId",
                table: "Gems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GemLots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TotalCost = table.Column<double>(nullable: false),
                    TotalAmountPaidToSeller = table.Column<double>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemLots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gems_GemLotId",
                table: "Gems",
                column: "GemLotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_GemLots_GemLotId",
                table: "Gems",
                column: "GemLotId",
                principalTable: "GemLots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_GemLots_GemLotId",
                table: "Gems");

            migrationBuilder.DropTable(
                name: "GemLots");

            migrationBuilder.DropIndex(
                name: "IX_Gems_GemLotId",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "GemLotId",
                table: "Gems");
        }
    }
}
