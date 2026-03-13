using AracIlan.Uygulama.Servisler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AracIlan.Altyapi.Servisler;

/// <summary>
/// Lokal dosya depolama - Production'da Azure Blob/S3 kullanılabilir
/// </summary>
public class DosyaServisi(IHostEnvironment ortam, IConfiguration yapilandirma) : IDosyaServisi
{
    private static readonly string[] IzinliUzantilar = [".jpg", ".jpeg", ".png", ".webp"];
    private const int MaksBoyutBytes = 5 * 1024 * 1024; // 5MB

    public async Task<string> AraçGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default)
    {
        var dosyaAdi2 = await KaydetAsync(dosya, dosyaAdi, kullaniciId, "araclar", iptal);
        return $"/uploads/araclar/{dosyaAdi2}";
    }

    public async Task<string> ExpertizGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default)
    {
        var dosyaAdi2 = await KaydetAsync(dosya, dosyaAdi, kullaniciId, "expertiz", iptal);
        return $"/uploads/expertiz/{dosyaAdi2}";
    }

    private async Task<string> KaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, string klasor, CancellationToken iptal)
    {
        var uzanti = Path.GetExtension(dosyaAdi).ToLowerInvariant();
        if (Array.IndexOf(IzinliUzantilar, uzanti) < 0)
            throw new ArgumentException($"İzinli formatlar: {string.Join(", ", IzinliUzantilar)}");

        if (dosya.Length > MaksBoyutBytes)
            throw new ArgumentException($"Maksimum dosya boyutu: {MaksBoyutBytes / 1024 / 1024}MB");

        var kok = yapilandirma["Dosya:KokDizin"] ?? Path.Combine(ortam.ContentRootPath, "uploads");
        var hedefKlasor = Path.Combine(kok, klasor);
        Directory.CreateDirectory(hedefKlasor);

        var benzersizAd = $"{Guid.NewGuid():N}{uzanti}";
        var tamYol = Path.Combine(hedefKlasor, benzersizAd);

        await using var fs = File.Create(tamYol);
        await dosya.CopyToAsync(fs, iptal);

        return benzersizAd;
    }
}
