using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class EvidencijaCasaIzmjena : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KursNaziv",
                table: "EvidencijeCasa",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KursNaziv",
                table: "EvidencijeCasa");
        }
    }
}
