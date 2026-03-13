namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Raporlama veri erişimi - Admin dashboard
/// </summary>
public interface IRaporDeposu
{
    Task<IReadOnlyList<MarkaIlanSayisi>> EnCokIlanliMarkalarAsync(int adet, CancellationToken iptal = default);
    Task<IReadOnlyList<ModelIlanSayisi>> EnCokIlanliModellerAsync(int adet, CancellationToken iptal = default);
    Task<IReadOnlyList<KategoriIlanSayisi>> KategoriBazliIlanSayilariAsync(CancellationToken iptal = default);
    Task<int> SonGunlerdeIlanSayisiAsync(int gun, CancellationToken iptal = default);
}

public record MarkaIlanSayisi(int MarkaId, string MarkaAd, int IlanSayisi);
public record ModelIlanSayisi(int ModelId, string ModelAd, string MarkaAd, int IlanSayisi);
public record KategoriIlanSayisi(string Kategori, int IlanSayisi);
