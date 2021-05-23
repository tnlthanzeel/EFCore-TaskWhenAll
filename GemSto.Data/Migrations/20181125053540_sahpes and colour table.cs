using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class sahpesandcolourtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_CertificateProvider_CertificateProviderId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Colour_ColourId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Gems_GemId",
                table: "Certificate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colour",
                table: "Colour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificateProvider",
                table: "CertificateProvider");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.RenameTable(
                name: "Colour",
                newName: "Colours");

            migrationBuilder.RenameTable(
                name: "CertificateProvider",
                newName: "CertificateProviders");

            migrationBuilder.RenameTable(
                name: "Certificate",
                newName: "Certificates");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_GemId",
                table: "Certificates",
                newName: "IX_Certificates_GemId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_ColourId",
                table: "Certificates",
                newName: "IX_Certificates_ColourId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_CertificateProviderId",
                table: "Certificates",
                newName: "IX_Certificates_CertificateProviderId");

            migrationBuilder.AddColumn<bool>(
                name: "IsTreated",
                table: "Gems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "Gems",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colours",
                table: "Colours",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificateProviders",
                table: "CertificateProviders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shape",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shape", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gems_ShapeId",
                table: "Gems",
                column: "ShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_CertificateProviders_CertificateProviderId",
                table: "Certificates",
                column: "CertificateProviderId",
                principalTable: "CertificateProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Colours_ColourId",
                table: "Certificates",
                column: "ColourId",
                principalTable: "Colours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Gems_GemId",
                table: "Certificates",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Shape_ShapeId",
                table: "Gems",
                column: "ShapeId",
                principalTable: "Shape",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_CertificateProviders_CertificateProviderId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Colours_ColourId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Gems_GemId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Shape_ShapeId",
                table: "Gems");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Shape");

            migrationBuilder.DropIndex(
                name: "IX_Gems_ShapeId",
                table: "Gems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colours",
                table: "Colours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificateProviders",
                table: "CertificateProviders");

            migrationBuilder.DropColumn(
                name: "IsTreated",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "Gems");

            migrationBuilder.RenameTable(
                name: "Colours",
                newName: "Colour");

            migrationBuilder.RenameTable(
                name: "Certificates",
                newName: "Certificate");

            migrationBuilder.RenameTable(
                name: "CertificateProviders",
                newName: "CertificateProvider");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_GemId",
                table: "Certificate",
                newName: "IX_Certificate_GemId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_ColourId",
                table: "Certificate",
                newName: "IX_Certificate_ColourId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_CertificateProviderId",
                table: "Certificate",
                newName: "IX_Certificate_CertificateProviderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colour",
                table: "Colour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificateProvider",
                table: "CertificateProvider",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_CertificateProvider_CertificateProviderId",
                table: "Certificate",
                column: "CertificateProviderId",
                principalTable: "CertificateProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Colour_ColourId",
                table: "Certificate",
                column: "ColourId",
                principalTable: "Colour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Gems_GemId",
                table: "Certificate",
                column: "GemId",
                principalTable: "Gems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
