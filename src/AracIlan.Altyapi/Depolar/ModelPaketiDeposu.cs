using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Model paketi veri deposu - Yıl girildiğinde o yıla ait paketler
/// </summary>
public class ModelPaketiDeposu : IModelPaketiDeposu
{
    private readonly AracIlanVeritabani _veritabani;

    public ModelPaketiDeposu(AracIlanVeritabani veritabani)
    {
        _veritabani = veritabani;
    }

    public async Task<List<ModelPaketi>> ModelVeYilIleGetirAsync(int modelId, int yil, CancellationToken iptal = default)
    {
        return await _veritabani.ModelPaketleri
            .Where(p => p.ModelId == modelId && yil >= p.BaslangicYili && yil <= p.BitisYili)
            .OrderBy(p => p.Ad)
            .ToListAsync(iptal);
    }

    public async Task<ModelPaketi?> IdIleGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.ModelPaketleri.FindAsync([id], iptal);
    }
}
