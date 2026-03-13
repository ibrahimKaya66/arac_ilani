namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// İlan liste öğesi - Arama sonuçlarında
/// </summary>
public record IlanListeYaniti(
    int Id,
    string Baslik,
    int UretimYili,
    int Kilometre,
    decimal Fiyat,
    string KapakGorselYolu,
    string MarkaAd,
    string ModelAd,
    string HasarDurumu
);
