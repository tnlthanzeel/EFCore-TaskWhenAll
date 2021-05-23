using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class deletedsllertableandaddedpaymentsstatustosellerandgemstatustogemstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gems_Sellers_SellerId",
                table: "Gems");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Gems_SellerId",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Gems");

            migrationBuilder.AddColumn<int>(
                name: "GemStatus",
                table: "Gems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusToSeller",
                table: "Gems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SellerName",
                table: "Gems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GemStatus",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "PaymentStatusToSeller",
                table: "Gems");

            migrationBuilder.DropColumn(
                name: "SellerName",
                table: "Gems");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Gems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gems_SellerId",
                table: "Gems",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gems_Sellers_SellerId",
                table: "Gems",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
