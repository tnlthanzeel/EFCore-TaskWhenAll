using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addedMiscPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MiscPayments",
                columns: table => new
                {
                    CreatedById = table.Column<string>(maxLength: 100, nullable: true),
                    EditedById = table.Column<string>(maxLength: 100, nullable: true),
                    EditedOn = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrimaryMiscCat = table.Column<int>(nullable: false),
                    SubMiscCat = table.Column<int>(nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ParticipantId = table.Column<int>(nullable: true),
                    ParticipantName = table.Column<string>(maxLength: 250, nullable: true),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscPayments_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiscPayments_MiscCategory_PrimaryMiscCat",
                        column: x => x.PrimaryMiscCat,
                        principalTable: "MiscCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiscPayments_MiscSubCategories_SubMiscCat",
                        column: x => x.SubMiscCat,
                        principalTable: "MiscSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MiscPayments_ParticipantId",
                table: "MiscPayments",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MiscPayments_PrimaryMiscCat",
                table: "MiscPayments",
                column: "PrimaryMiscCat");

            migrationBuilder.CreateIndex(
                name: "IX_MiscPayments_SubMiscCat",
                table: "MiscPayments",
                column: "SubMiscCat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MiscPayments");
        }
    }
}
