using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// Model paketi iş mantığı - Yıl girildiğinde o yıla ait paketler
/// </summary>
public class ModelPaketiServisi
{
    private readonly IModelPaketiDeposu _paketDeposu;

    public ModelPaketiServisi(IModelPaketiDeposu paketDeposu)
    {
        _paketDeposu = paketDeposu;
    }

    public async Task<List<ModelPaketiYaniti>> ModelVeYilIleGetirAsync(int modelId, int yil, CancellationToken iptal = default)
    {
        var paketler = await _paketDeposu.ModelVeYilIleGetirAsync(modelId, yil, iptal);
        return paketler.Select(p => new ModelPaketiYaniti(
            p.Id, p.Ad, p.BaslangicYili, p.BitisYili)).ToList();
    }
}
