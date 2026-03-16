using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Araç ilanı veri erişim arayüzü
/// </summary>
public interface IAracDeposu
{
    Task<Arac?> IdIleGetirAsync(int id, CancellationToken iptal = default);
    Task<Arac?> IdVeDetaylarlaGetirAsync(int id, CancellationToken iptal = default);
    Task<List<Arac>> FiltreliGetirAsync(AracFiltre filtre, CancellationToken iptal = default);
    Task<int> FiltreliSayiAsync(AracFiltre filtre, CancellationToken iptal = default);
    Task<List<Arac>> KullaniciIlanlariniGetirAsync(string kullaniciId, int sayfa, int sayfaBoyutu, CancellationToken iptal = default);
    Task<List<Arac>> KullaniciIlanlariniFiltreliGetirAsync(string kullaniciId, AracFiltre filtre, CancellationToken iptal = default);
    Task<int> KullaniciIlanSayisiAsync(string kullaniciId, CancellationToken iptal = default);
    Task<int> KullaniciIlanFiltreliSayiAsync(string kullaniciId, AracFiltre filtre, CancellationToken iptal = default);
    Task<Arac> EkleAsync(Arac arac, CancellationToken iptal = default);
    Task GuncelleAsync(Arac arac, CancellationToken iptal = default);
    Task<bool> SilAsync(int id, string kullaniciId, CancellationToken iptal = default);
}

/// <summary>
/// İlan listeleme filtresi - API'den gelecek parametreler
/// </summary>
public class AracFiltre
{
    public AracKategorisi? Kategori { get; set; }
    public int? MarkaId { get; set; }
    public int? ModelId { get; set; }
    public int? MinYil { get; set; }
    public int? MaxYil { get; set; }
    public decimal? MinFiyat { get; set; }
    public decimal? MaxFiyat { get; set; }
    public int? MinKm { get; set; }
    public int? MaxKm { get; set; }
    public int Sayfa { get; set; } = 1;
    public int SayfaBoyutu { get; set; } = 20;
    public string? Siralama { get; set; } // "fiyat", "yil", "km", "tarih"
}
