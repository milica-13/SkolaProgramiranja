using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class DodavanjeProfila : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Obrazovanje",
                table: "Korisnici",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Biografija",
                table: "Korisnici",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlikaProfilaPath",
                table: "Korisnici",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Korisnici",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlikaProfilaPath",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Korisnici");

            migrationBuilder.AlterColumn<string>(
                name: "Obrazovanje",
                table: "Korisnici",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Biografija",
                table: "Korisnici",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
