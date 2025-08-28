using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class EvidencijaCasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvidencijeCasa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstruktorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TemaCasa = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    PrisutniUcenici = table.Column<string>(type: "TEXT", nullable: false),
                    TrajanjeMin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidencijeCasa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidencijeCasa_Korisnici_InstruktorId",
                        column: x => x.InstruktorId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidencijeCasa_InstruktorId",
                table: "EvidencijeCasa",
                column: "InstruktorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidencijeCasa");
        }
    }
}
