using Microsoft.EntityFrameworkCore.Migrations;

namespace Template.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prodavnica",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    brojStolova = table.Column<int>(type: "int", nullable: false),
                    Zarada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnica", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sastojak",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojak", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Proizvod",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    ProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvod", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Proizvod_Prodavnica_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdavnicaSastojak",
                columns: table => new
                {
                    ProdavnicaID = table.Column<int>(type: "int", nullable: false),
                    SastojakID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdavnicaSastojak", x => new { x.ProdavnicaID, x.SastojakID });
                    table.ForeignKey(
                        name: "FK_ProdavnicaSastojak_Prodavnica_ProdavnicaID",
                        column: x => x.ProdavnicaID,
                        principalTable: "Prodavnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdavnicaSastojak_Sastojak_SastojakID",
                        column: x => x.SastojakID,
                        principalTable: "Sastojak",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProizvodSastojak",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    ProizvodID = table.Column<int>(type: "int", nullable: true),
                    SastojakID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProizvodSastojak", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProizvodSastojak_Proizvod_ProizvodID",
                        column: x => x.ProizvodID,
                        principalTable: "Proizvod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProizvodSastojak_Sastojak_SastojakID",
                        column: x => x.SastojakID,
                        principalTable: "Sastojak",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdavnicaSastojak_SastojakID",
                table: "ProdavnicaSastojak",
                column: "SastojakID");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvod_ProdavnicaID",
                table: "Proizvod",
                column: "ProdavnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodSastojak_ProizvodID",
                table: "ProizvodSastojak",
                column: "ProizvodID");

            migrationBuilder.CreateIndex(
                name: "IX_ProizvodSastojak_SastojakID",
                table: "ProizvodSastojak",
                column: "SastojakID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdavnicaSastojak");

            migrationBuilder.DropTable(
                name: "ProizvodSastojak");

            migrationBuilder.DropTable(
                name: "Proizvod");

            migrationBuilder.DropTable(
                name: "Sastojak");

            migrationBuilder.DropTable(
                name: "Prodavnica");
        }
    }
}
