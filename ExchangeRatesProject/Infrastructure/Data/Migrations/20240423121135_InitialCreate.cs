using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRatesProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISLEM_TARIHI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KULLANICI_IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KUR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YUZDESEL_DEGISIM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HATA_FONKSIYON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HATA_ACIKLAMASI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
