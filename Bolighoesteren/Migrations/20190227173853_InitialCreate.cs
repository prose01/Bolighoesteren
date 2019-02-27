using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bolighoesteren.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ejendomme",
                columns: table => new
                {
                    PropertyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Postnummer = table.Column<int>(nullable: false),
                    Adresse = table.Column<string>(nullable: true),
                    Pris = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Areal = table.Column<int>(nullable: false),
                    GrundAreal = table.Column<int>(nullable: false),
                    Rum = table.Column<int>(nullable: false),
                    Byggeår = table.Column<int>(nullable: false),
                    Liggetid = table.Column<string>(nullable: true),
                    Prisudvikling = table.Column<string>(nullable: true),
                    KvadratmeterPris = table.Column<string>(nullable: true),
                    Ejerudgifter = table.Column<string>(nullable: true),
                    BoligydForbrugsafh = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    HashCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejendomme", x => x.PropertyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ejendomme_Postnummer",
                table: "Ejendomme",
                column: "Postnummer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ejendomme");
        }
    }
}
