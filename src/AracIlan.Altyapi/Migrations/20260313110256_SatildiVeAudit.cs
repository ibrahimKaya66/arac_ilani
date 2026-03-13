using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AracIlan.Altyapi.Migrations
{
    /// <inheritdoc />
    public partial class SatildiVeAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelPaketleri_Modeller_ModelId",
                table: "ModelPaketleri");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorSecenekleri_ModelPaketleri_ModelPaketiId",
                table: "MotorSecenekleri");

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "UyelikPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "UyelikPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "UyelikPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "UyelikPaketleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "MotorSecenekleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "MotorSecenekleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "MotorSecenekleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "MotorSecenekleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "ModelPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "ModelPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "ModelPaketleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "ModelPaketleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "Modeller",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "Modeller",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "Modeller",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "Modeller",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "Markalar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "Markalar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "Markalar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "Markalar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "KullaniciAbonelikleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "KullaniciAbonelikleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "KullaniciAbonelikleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "KullaniciAbonelikleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "ExpertizRaporlari",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "ExpertizRaporlari",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "ExpertizRaporlari",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "ExpertizRaporlari",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "Araclar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "Araclar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "Araclar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SatildiTarihi",
                table: "Araclar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "Araclar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimEklendi",
                table: "AracGorselleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimGuncelledi",
                table: "AracGorselleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KimSildi",
                table: "AracGorselleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SilinmeTarihi",
                table: "AracGorselleri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller",
                column: "MarkaId",
                principalTable: "Markalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelPaketleri_Modeller_ModelId",
                table: "ModelPaketleri",
                column: "ModelId",
                principalTable: "Modeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotorSecenekleri_ModelPaketleri_ModelPaketiId",
                table: "MotorSecenekleri",
                column: "ModelPaketiId",
                principalTable: "ModelPaketleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelPaketleri_Modeller_ModelId",
                table: "ModelPaketleri");

            migrationBuilder.DropForeignKey(
                name: "FK_MotorSecenekleri_ModelPaketleri_ModelPaketiId",
                table: "MotorSecenekleri");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "UyelikPaketleri");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "UyelikPaketleri");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "UyelikPaketleri");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "UyelikPaketleri");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "MotorSecenekleri");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "MotorSecenekleri");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "MotorSecenekleri");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "MotorSecenekleri");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "ModelPaketleri");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "ModelPaketleri");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "ModelPaketleri");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "ModelPaketleri");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "Markalar");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "Markalar");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "Markalar");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "Markalar");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "KullaniciAbonelikleri");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "KullaniciAbonelikleri");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "KullaniciAbonelikleri");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "KullaniciAbonelikleri");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "ExpertizRaporlari");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "ExpertizRaporlari");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "ExpertizRaporlari");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "ExpertizRaporlari");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "Araclar");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "Araclar");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "Araclar");

            migrationBuilder.DropColumn(
                name: "SatildiTarihi",
                table: "Araclar");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "Araclar");

            migrationBuilder.DropColumn(
                name: "KimEklendi",
                table: "AracGorselleri");

            migrationBuilder.DropColumn(
                name: "KimGuncelledi",
                table: "AracGorselleri");

            migrationBuilder.DropColumn(
                name: "KimSildi",
                table: "AracGorselleri");

            migrationBuilder.DropColumn(
                name: "SilinmeTarihi",
                table: "AracGorselleri");

            migrationBuilder.AddForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller",
                column: "MarkaId",
                principalTable: "Markalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelPaketleri_Modeller_ModelId",
                table: "ModelPaketleri",
                column: "ModelId",
                principalTable: "Modeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MotorSecenekleri_ModelPaketleri_ModelPaketiId",
                table: "MotorSecenekleri",
                column: "ModelPaketiId",
                principalTable: "ModelPaketleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
