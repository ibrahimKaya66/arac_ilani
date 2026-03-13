using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracIlan.Altyapi.Migrations
{
    /// <inheritdoc />
    public partial class IlkMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UyelikPaketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IlanHakki = table.Column<int>(type: "int", nullable: false),
                    MaksimumFotograf = table.Column<int>(type: "int", nullable: false),
                    IlanSuresiGun = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyelikPaketleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UretimBaslangicYili = table.Column<int>(type: "int", nullable: false),
                    UretimBitisYili = table.Column<int>(type: "int", nullable: false),
                    MarkaId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modeller_Markalar_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Markalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciAbonelikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UyelikPaketiId = table.Column<int>(type: "int", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KullanilanIlanHakki = table.Column<int>(type: "int", nullable: false),
                    OdemeReferansi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciAbonelikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciAbonelikleri_UyelikPaketleri_UyelikPaketiId",
                        column: x => x.UyelikPaketiId,
                        principalTable: "UyelikPaketleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModelPaketleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaslangicYili = table.Column<int>(type: "int", nullable: false),
                    BitisYili = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelPaketleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelPaketleri_Modeller_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Modeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotorSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotorHacmi = table.Column<int>(type: "int", nullable: true),
                    YakitTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Guc = table.Column<int>(type: "int", nullable: true),
                    ModelPaketiId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorSecenekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotorSecenekleri_ModelPaketleri_ModelPaketiId",
                        column: x => x.ModelPaketiId,
                        principalTable: "ModelPaketleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    UretimYili = table.Column<int>(type: "int", nullable: false),
                    Kilometre = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Renk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VitesTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasarDurumu = table.Column<int>(type: "int", nullable: false),
                    IlanDurumu = table.Column<int>(type: "int", nullable: false),
                    IlanBitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KullaniciId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotorSecenegiId = table.Column<int>(type: "int", nullable: false),
                    TeknikOzelliklerJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Araclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Araclar_MotorSecenekleri_MotorSecenegiId",
                        column: x => x.MotorSecenegiId,
                        principalTable: "MotorSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AracGorselleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracGorselleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracGorselleri_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpertizRaporlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GorselYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AIAnalizSonucu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AracId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertizRaporlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertizRaporlari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AracGorselleri_AracId",
                table: "AracGorselleri",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_Araclar_MotorSecenegiId",
                table: "Araclar",
                column: "MotorSecenegiId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertizRaporlari_AracId",
                table: "ExpertizRaporlari",
                column: "AracId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciAbonelikleri_UyelikPaketiId",
                table: "KullaniciAbonelikleri",
                column: "UyelikPaketiId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeller_MarkaId",
                table: "Modeller",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelPaketleri_ModelId",
                table: "ModelPaketleri",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MotorSecenekleri_ModelPaketiId",
                table: "MotorSecenekleri",
                column: "ModelPaketiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AracGorselleri");

            migrationBuilder.DropTable(
                name: "ExpertizRaporlari");

            migrationBuilder.DropTable(
                name: "KullaniciAbonelikleri");

            migrationBuilder.DropTable(
                name: "Araclar");

            migrationBuilder.DropTable(
                name: "UyelikPaketleri");

            migrationBuilder.DropTable(
                name: "MotorSecenekleri");

            migrationBuilder.DropTable(
                name: "ModelPaketleri");

            migrationBuilder.DropTable(
                name: "Modeller");

            migrationBuilder.DropTable(
                name: "Markalar");
        }
    }
}
