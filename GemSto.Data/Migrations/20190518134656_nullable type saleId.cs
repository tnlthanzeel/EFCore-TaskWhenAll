using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class nullabletypesaleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Sales_SaleId",
                table: "GemSales");

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "GemSales",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Sales_SaleId",
                table: "GemSales",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Sales_SaleId",
                table: "GemSales");

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "GemSales",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Sales_SaleId",
                table: "GemSales",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
