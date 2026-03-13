namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Marka listesi/ detay yanıtı - API'den dönecek
/// </summary>
public record MarkaYaniti(
    int Id,
    string Ad,
    string Slug,
    int Sira
);
