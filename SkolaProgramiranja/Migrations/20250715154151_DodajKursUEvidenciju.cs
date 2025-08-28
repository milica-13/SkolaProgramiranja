using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class DodajKursUEvidenciju : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KursId",
                table: "EvidencijeCasa",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EvidencijeCasa_KursId",
                table: "EvidencijeCasa",
                column: "KursId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvidencijeCasa_Kursevi_KursId",
                table: "EvidencijeCasa",
                column: "KursId",
                principalTable: "Kursevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvidencijeCasa_Kursevi_KursId",
                table: "EvidencijeCasa");

            migrationBuilder.DropIndex(
                name: "IX_EvidencijeCasa_KursId",
                table: "EvidencijeCasa");

            migrationBuilder.DropColumn(
                name: "KursId",
                table: "EvidencijeCasa");
        }
    }
}
