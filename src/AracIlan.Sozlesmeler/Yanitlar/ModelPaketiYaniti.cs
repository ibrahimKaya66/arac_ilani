namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Model paketi yanıtı - Yıl girildiğinde gelen paketler
/// </summary>
public record ModelPaketiYaniti(
    int Id,
    string Ad,
    int BaslangicYili,
    int BitisYili
);
