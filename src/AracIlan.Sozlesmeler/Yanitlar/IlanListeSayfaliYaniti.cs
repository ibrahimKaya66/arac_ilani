namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Sayfalanmış ilan listesi yanıtı
/// </summary>
public record IlanListeSayfaliYaniti(
    List<IlanListeYaniti> Ilanlar,
    int ToplamKayit,
    int Sayfa,
    int SayfaBoyutu
);
