using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class addthirdpartycertification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThirdPartyCertificates",
                columns: table => new
                {
                    CreatedById = table.Column<string>(nullable: true),
                    EditedById = table.Column<string>(nullable: true),
                    EditedOn = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColourId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: true),
                    IsTreated = table.Column<bool>(nullable: false),
                    CertifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Length = table.Column<decimal>(nullable: true),
                    Width = table.Column<decimal>(nullable: true),
                    Depth = table.Column<decimal>(nullable: true),
                    ShapeId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: true),
                    CertificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdPartyCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThirdPartyCertificates_Certifications_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "Certifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThirdPartyCertificates_Colours_ColourId",
                        column: x => x.ColourId,
                        principalTable: "Colours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThirdPartyCertificates_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThirdPartyCertificates_Shapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_CertificationId",
                table: "ThirdPartyCertificates",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_ColourId",
                table: "ThirdPartyCertificates",
                column: "ColourId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_OriginId",
                table: "ThirdPartyCertificates",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyCertificates_ShapeId",
                table: "ThirdPartyCertificates",
                column: "ShapeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirdPartyCertificates");
        }
    }
}
