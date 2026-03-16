using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// Model iş mantığı - Marka seçildiğinde modeller
/// </summary>
public class ModelServisi
{
    private readonly IModelDeposu _modelDeposu;

    public ModelServisi(IModelDeposu modelDeposu)
    {
        _modelDeposu = modelDeposu;
    }

    public async Task<List<ModelYaniti>> MarkaIdIleGetirAsync(int markaId, CancellationToken iptal = default)
    {
        var modeller = await _modelDeposu.MarkaIdIleGetirAsync(markaId, iptal);
        return modeller.OrderBy(m => m.Ad, StringComparer.OrdinalIgnoreCase)
            .Select(m => new ModelYaniti(
                m.Id, m.Ad, m.Slug, m.UretimBaslangicYili, m.UretimBitisYili)).ToList();
    }
}
