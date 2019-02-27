using Microsoft.EntityFrameworkCore.Migrations;

namespace Bolighoesteren.Migrations
{
    public partial class updatePropety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Liggetid",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Liggetid",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
