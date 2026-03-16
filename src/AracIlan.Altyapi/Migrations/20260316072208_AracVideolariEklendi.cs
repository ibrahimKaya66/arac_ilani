using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracIlan.Altyapi.Migrations
{
    /// <inheritdoc />
    public partial class AracVideolariEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AracVideolari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    KimEklendi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KimGuncelledi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false),
                    KimSildi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SilinmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracVideolari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracVideolari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AracVideolari_AracId",
                table: "AracVideolari",
                column: "AracId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AracVideolari");
        }
    }
}
