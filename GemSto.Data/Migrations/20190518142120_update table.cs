using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class updatetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalePayments_Sales_SaleId",
                table: "SalePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Buyers_BuyerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_BuyerId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Sales");

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "SalePayments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "GemSalesId",
                table: "SalePayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BuyerId",
                table: "GemSales",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPieces",
                table: "GemSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "GemSales",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalePayments_GemSalesId",
                table: "SalePayments",
                column: "GemSalesId");

            migrationBuilder.CreateIndex(
                name: "IX_GemSales_BuyerId",
                table: "GemSales",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GemSales_Buyers_BuyerId",
                table: "GemSales",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePayments_GemSales_GemSalesId",
                table: "SalePayments",
                column: "GemSalesId",
                principalTable: "GemSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePayments_Sales_SaleId",
                table: "SalePayments",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GemSales_Buyers_BuyerId",
                table: "GemSales");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePayments_GemSales_GemSalesId",
                table: "SalePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePayments_Sales_SaleId",
                table: "SalePayments");

            migrationBuilder.DropIndex(
                name: "IX_SalePayments_GemSalesId",
                table: "SalePayments");

            migrationBuilder.DropIndex(
                name: "IX_GemSales_BuyerId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "GemSalesId",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "NumberOfPieces",
                table: "GemSales");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "GemSales");

            migrationBuilder.AddColumn<Guid>(
                name: "BuyerId",
                table: "Sales",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "SalePayments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BuyerId",
                table: "Sales",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalePayments_Sales_SaleId",
                table: "SalePayments",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Buyers_BuyerId",
                table: "Sales",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
