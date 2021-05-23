using Microsoft.EntityFrameworkCore.Migrations;

namespace GemSto.Data.Migrations
{
    public partial class lookuptablesmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CertificateProviders",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Varieties",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Shapes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Shapes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sellers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Colours",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "CertificateProviders",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "CertificateProviders");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CertificateProviders",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Varieties",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Shapes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sellers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Colours",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
