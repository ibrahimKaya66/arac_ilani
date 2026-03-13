using AracIlan.Alan.Sabitler;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Yeni ilan oluşturma isteği
/// Akıllı form: Marka→Model→Yıl→Paket→Motor seçildikten sonra
/// </summary>
public record IlanOlusturmaIstegi(
    AracKategorisi Kategori,
    int MotorSecenegiId,
    int UretimYili,
    int Kilometre,
    decimal Fiyat,
    string Renk,
    string VitesTipi,
    string Aciklama,
    HasarDurumu HasarDurumu,
    List<string>? GorselYollari = null,
    string? ExpertizGorselYolu = null
);
