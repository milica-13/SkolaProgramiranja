using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class MapKorisnikKurs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kursevi_Korisnici_InstruktorId",
                table: "Kursevi");

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.CreateTable(
                name: "KorisnikKurs",
                columns: table => new
                {
                    KorisniciId = table.Column<int>(type: "INTEGER", nullable: false),
                    KurseviId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikKurs", x => new { x.KorisniciId, x.KurseviId });
                    table.ForeignKey(
                        name: "FK_KorisnikKurs_Korisnici_KorisniciId",
                        column: x => x.KorisniciId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikKurs_Kursevi_KurseviId",
                        column: x => x.KurseviId,
                        principalTable: "Kursevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikKurs_KurseviId",
                table: "KorisnikKurs",
                column: "KurseviId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kursevi_Korisnici_InstruktorId",
                table: "Kursevi",
                column: "InstruktorId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kursevi_Korisnici_InstruktorId",
                table: "Kursevi");

            migrationBuilder.DropTable(
                name: "KorisnikKurs");

            migrationBuilder.InsertData(
                table: "Korisnici",
                columns: new[] { "Id", "Biografija", "Email", "Ime", "Lozinka", "MoraPromijenitiLozinku", "Obrazovanje", "Prezime", "SlikaProfilaPath", "Telefon", "Uloga" },
                values: new object[,]
                {
                    { 101, null, "ucenik1@skola.com", "Ucenik1", "test123", false, null, "Ucenik1", "profilna.png", null, "Ucenik" },
                    { 102, null, "ucenik2@skola.com", "Ucenik2", "test123", false, null, "Ucenik2", "profilna.png", null, "Ucenik" },
                    { 103, null, "ucenik3@skola.com", "Ucenik3", "test123", false, null, "Ucenik3", "profilna.png", null, "Ucenik" },
                    { 104, null, "ucenik4@skola.com", "Ucenik4", "test123", false, null, "Ucenik4", "profilna.png", null, "Ucenik" },
                    { 105, null, "ucenik5@skola.com", "Ucenik5", "test123", false, null, "Ucenik5", "profilna.png", null, "Ucenik" },
                    { 106, null, "ucenik6@skola.com", "Ucenik6", "test123", false, null, "Ucenik6", "profilna.png", null, "Ucenik" },
                    { 107, null, "ucenik7@skola.com", "Ucenik7", "test123", false, null, "Ucenik7", "profilna.png", null, "Ucenik" },
                    { 108, null, "ucenik8@skola.com", "Ucenik8", "test123", false, null, "Ucenik8", "profilna.png", null, "Ucenik" },
                    { 109, null, "ucenik9@skola.com", "Ucenik9", "test123", false, null, "Ucenik9", "profilna.png", null, "Ucenik" },
                    { 110, null, "ucenik10@skola.com", "Ucenik10", "test123", false, null, "Ucenik10", "profilna.png", null, "Ucenik" },
                    { 111, null, "ucenik11@skola.com", "Ucenik11", "test123", false, null, "Ucenik11", "profilna.png", null, "Ucenik" },
                    { 112, null, "ucenik12@skola.com", "Ucenik12", "test123", false, null, "Ucenik12", "profilna.png", null, "Ucenik" },
                    { 113, null, "ucenik13@skola.com", "Ucenik13", "test123", false, null, "Ucenik13", "profilna.png", null, "Ucenik" },
                    { 114, null, "ucenik14@skola.com", "Ucenik14", "test123", false, null, "Ucenik14", "profilna.png", null, "Ucenik" },
                    { 115, null, "ucenik15@skola.com", "Ucenik15", "test123", false, null, "Ucenik15", "profilna.png", null, "Ucenik" },
                    { 116, null, "ucenik16@skola.com", "Ucenik16", "test123", false, null, "Ucenik16", "profilna.png", null, "Ucenik" },
                    { 117, null, "ucenik17@skola.com", "Ucenik17", "test123", false, null, "Ucenik17", "profilna.png", null, "Ucenik" },
                    { 118, null, "ucenik18@skola.com", "Ucenik18", "test123", false, null, "Ucenik18", "profilna.png", null, "Ucenik" },
                    { 119, null, "ucenik19@skola.com", "Ucenik19", "test123", false, null, "Ucenik19", "profilna.png", null, "Ucenik" },
                    { 120, null, "ucenik20@skola.com", "Ucenik20", "test123", false, null, "Ucenik20", "profilna.png", null, "Ucenik" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Kursevi_Korisnici_InstruktorId",
                table: "Kursevi",
                column: "InstruktorId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
