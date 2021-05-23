using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class paymentlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Miscellaneous",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaidTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miscellaneous", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiscellaneousPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    PaidOn = table.Column<DateTimeOffset>(nullable: false),
                    MiscellaneousId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscellaneousPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscellaneousPayments_Miscellaneous_MiscellaneousId",
                        column: x => x.MiscellaneousId,
                        principalTable: "Miscellaneous",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MiscellaneousPayments_MiscellaneousId",
                table: "MiscellaneousPayments",
                column: "MiscellaneousId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MiscellaneousPayments");

            migrationBuilder.DropTable(
                name: "Miscellaneous");
        }
    }
}
