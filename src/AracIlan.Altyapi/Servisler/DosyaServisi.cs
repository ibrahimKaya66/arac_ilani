using AracIlan.Uygulama.Servisler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AracIlan.Altyapi.Servisler;

/// <summary>
/// Lokal dosya depolama - Production'da Azure Blob/S3 kullanılabilir
/// </summary>
public class DosyaServisi(IHostEnvironment ortam, IConfiguration yapilandirma) : IDosyaServisi
{
    private static readonly string[] IzinliGorselUzantilari = [".jpg", ".jpeg", ".png", ".webp"];
    private static readonly string[] IzinliVideoUzantilari = [".mp4", ".webm"];
    private const int MaksGorselBoyutBytes = 5 * 1024 * 1024; // 5MB
    private const int MaksVideoBoyutBytes = 50 * 1024 * 1024; // 50MB

    public async Task<string> AraçGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default)
    {
        var dosyaAdi2 = await KaydetAsync(dosya, dosyaAdi, kullaniciId, "araclar", IzinliGorselUzantilari, MaksGorselBoyutBytes, iptal);
        return $"/uploads/araclar/{dosyaAdi2}";
    }

    public async Task<string> ExpertizGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default)
    {
        var dosyaAdi2 = await KaydetAsync(dosya, dosyaAdi, kullaniciId, "expertiz", IzinliGorselUzantilari, MaksGorselBoyutBytes, iptal);
        return $"/uploads/expertiz/{dosyaAdi2}";
    }

    public async Task<string> AracVideosuKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default)
    {
        var dosyaAdi2 = await KaydetAsync(dosya, dosyaAdi, kullaniciId, "videolar", IzinliVideoUzantilari, MaksVideoBoyutBytes, iptal);
        return $"/uploads/videolar/{dosyaAdi2}";
    }

    private async Task<string> KaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, string klasor, string[] izinliUzantilar, int maksBoyut, CancellationToken iptal)
    {
        var uzanti = Path.GetExtension(dosyaAdi).ToLowerInvariant();
        if (Array.IndexOf(izinliUzantilar, uzanti) < 0)
            throw new ArgumentException($"İzinli formatlar: {string.Join(", ", izinliUzantilar)}");

        if (dosya.Length > maksBoyut)
            throw new ArgumentException($"Maksimum dosya boyutu: {maksBoyut / 1024 / 1024}MB");

        var kok = yapilandirma["Dosya:KokDizin"] ?? Path.Combine(ortam.ContentRootPath, "uploads");
        var hedefKlasor = Path.Combine(kok, klasor);
        Directory.CreateDirectory(hedefKlasor);

        var benzersizAd = $"{Guid.NewGuid():N}{uzanti}";
        var tamYol = Path.Combine(hedefKlasor, benzersizAd);

        await using var fs = File.Create(tamYol);
        await dosya.CopyToAsync(fs, iptal);

        return benzersizAd;
    }

    public void DosyaSil(string gorselYolu)
    {
        if (string.IsNullOrWhiteSpace(gorselYolu) || !gorselYolu.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
            return;

        var rel = gorselYolu.TrimStart('/');
        if (rel.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            rel = rel["uploads/".Length..];

        var kok = yapilandirma["Dosya:KokDizin"] ?? Path.Combine(ortam.ContentRootPath, "uploads");
        var tamYol = Path.GetFullPath(Path.Combine(kok, rel.Replace("/", Path.DirectorySeparatorChar.ToString())));
        var kokFull = Path.GetFullPath(kok);

        if (!tamYol.StartsWith(kokFull, StringComparison.OrdinalIgnoreCase))
            return;

        try
        {
            if (File.Exists(tamYol))
                File.Delete(tamYol);
        }
        catch
        {
            // Silme hatası - loglanabilir, sessizce geç
        }
    }
}
