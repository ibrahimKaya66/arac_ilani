namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Model listesi yanıtı - Yıl aralığı ile
/// </summary>
public record ModelYaniti(
    int Id,
    string Ad,
    string Slug,
    int UretimBaslangicYili,
    int UretimBitisYili
);
