using Microsoft.EntityFrameworkCore.Migrations;

namespace Bolighoesteren.Migrations
{
    public partial class newPropety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_Postcode",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Properties",
                newName: "Prisudvikling");

            migrationBuilder.RenameColumn(
                name: "Postcode",
                table: "Properties",
                newName: "Pris");

            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Properties",
                newName: "KvadratmeterPris");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Properties",
                newName: "Foto");

            migrationBuilder.AlterColumn<string>(
                name: "Pris",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Areal",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BoligydForbrugsafh",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Byggeår",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ejerudgifter",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GrundAreal",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Liggetid",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Postnummer",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rum",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Postnummer",
                table: "Properties",
                column: "Postnummer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_Postnummer",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Areal",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BoligydForbrugsafh",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Byggeår",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Ejerudgifter",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "GrundAreal",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Liggetid",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Postnummer",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Rum",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "Prisudvikling",
                table: "Properties",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Pris",
                table: "Properties",
                newName: "Postcode");

            migrationBuilder.RenameColumn(
                name: "KvadratmeterPris",
                table: "Properties",
                newName: "Photo");

            migrationBuilder.RenameColumn(
                name: "Foto",
                table: "Properties",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Postcode",
                table: "Properties",
                column: "Postcode");
        }
    }
}
