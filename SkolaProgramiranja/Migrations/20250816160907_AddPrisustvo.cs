using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class AddPrisustvo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prisustva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvidencijaCasaId = table.Column<int>(type: "INTEGER", nullable: false),
                    UcenikId = table.Column<int>(type: "INTEGER", nullable: false),
                    Prisutan = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisustva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prisustva_EvidencijeCasa_EvidencijaCasaId",
                        column: x => x.EvidencijaCasaId,
                        principalTable: "EvidencijeCasa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prisustva_Korisnici_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_EvidencijaCasaId_UcenikId",
                table: "Prisustva",
                columns: new[] { "EvidencijaCasaId", "UcenikId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_UcenikId",
                table: "Prisustva",
                column: "UcenikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prisustva");
        }
    }
}
