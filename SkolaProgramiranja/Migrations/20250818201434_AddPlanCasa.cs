using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkolaProgramiranja.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanCasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanoviCasa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KursId = table.Column<int>(type: "INTEGER", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tema = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoviCasa", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanoviCasa_KursId_Datum",
                table: "PlanoviCasa",
                columns: new[] { "KursId", "Datum" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanoviCasa");
        }
    }
}
