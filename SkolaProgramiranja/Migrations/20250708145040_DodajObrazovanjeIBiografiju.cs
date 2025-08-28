using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    
    public partial class DodajObrazovanjeIBiografiju : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biografija", 
                table: "Korisnici",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Obrazovanje",
                table: "Korisnici",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biografija",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "Obrazovanje",
                table: "Korisnici");
        }
    }
}
