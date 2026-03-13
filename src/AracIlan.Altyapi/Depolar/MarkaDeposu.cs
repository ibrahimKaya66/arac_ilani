using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Marka veri deposu implementasyonu
/// </summary>
public class MarkaDeposu : IMarkaDeposu
{
    private readonly AracIlanVeritabani _veritabani;

    public MarkaDeposu(AracIlanVeritabani veritabani)
    {
        _veritabani = veritabani;
    }

    public async Task<List<Marka>> KategoriyeGoreGetirAsync(AracKategorisi kategori, CancellationToken iptal = default)
    {
        return await _veritabani.Markalar
            .Where(m => m.Kategori == kategori)
            .OrderBy(m => m.Sira)
            .ThenBy(m => m.Ad)
            .ToListAsync(iptal);
    }

    public async Task<Marka?> IdIleGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.Markalar.FindAsync([id], iptal);
    }
}
