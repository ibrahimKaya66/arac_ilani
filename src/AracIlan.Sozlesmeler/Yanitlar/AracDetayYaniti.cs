namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// İlan detay yanıtı - Tek ilan sayfası
/// </summary>
public record AracDetayYaniti(
    int Id,
    string Baslik,
    string Aciklama,
    string Kategori,
    int UretimYili,
    int Kilometre,
    decimal Fiyat,
    string Renk,
    string VitesTipi,
    string HasarDurumu,
    string MarkaAd,
    string ModelAd,
    string MotorAd,
    List<string> GorselYollari,
    string? TeknikOzelliklerJson,
    string? ExpertizAIAnalizi,
    string IlanDurumu,
    string? KullaniciId
);
