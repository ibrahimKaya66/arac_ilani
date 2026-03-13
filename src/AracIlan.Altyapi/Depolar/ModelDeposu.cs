using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Model veri deposu implementasyonu
/// </summary>
public class ModelDeposu : IModelDeposu
{
    private readonly AracIlanVeritabani _veritabani;

    public ModelDeposu(AracIlanVeritabani veritabani)
    {
        _veritabani = veritabani;
    }

    public async Task<List<Model>> MarkaIdIleGetirAsync(int markaId, CancellationToken iptal = default)
    {
        return await _veritabani.Modeller
            .Where(m => m.MarkaId == markaId)
            .OrderBy(m => m.Ad)
            .ToListAsync(iptal);
    }

    public async Task<Model?> IdIleGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.Modeller.FindAsync([id], iptal);
    }
}
