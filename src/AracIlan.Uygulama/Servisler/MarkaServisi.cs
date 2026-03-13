using AracIlan.Alan.Sabitler;
using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// Marka iş mantığı - Kategoriye göre markalar
/// </summary>
public class MarkaServisi
{
    private readonly IMarkaDeposu _markaDeposu;

    public MarkaServisi(IMarkaDeposu markaDeposu)
    {
        _markaDeposu = markaDeposu;
    }

    public async Task<List<MarkaYaniti>> KategoriyeGoreGetirAsync(AracKategorisi kategori, CancellationToken iptal = default)
    {
        var markalar = await _markaDeposu.KategoriyeGoreGetirAsync(kategori, iptal);
        return markalar.Select(m => new MarkaYaniti(m.Id, m.Ad, m.Slug, m.Sira)).ToList();
    }
}
