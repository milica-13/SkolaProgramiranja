using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
        
    public partial class DodajMoraPromijenitiLozinku : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MoraPromijenitiLozinku",
                table: "Korisnici",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoraPromijenitiLozinku",
                table: "Korisnici");
        }
    }
}
