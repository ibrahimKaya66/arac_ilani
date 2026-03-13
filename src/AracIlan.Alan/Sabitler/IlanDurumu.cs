namespace AracIlan.Alan.Sabitler;

/// <summary>
/// İlanın yayın durumu
/// </summary>
public enum IlanDurumu
{
    /// <summary>Henüz yayınlanmamış taslak</summary>
    Taslak = 0,

    /// <summary>Aktif yayında</summary>
    Yayinda = 1,

    /// <summary>Süresi doldu</summary>
    SuresiDoldu = 2,

    /// <summary>Kullanıcı tarafından kaldırıldı</summary>
    Kaldirildi = 3,

    /// <summary>Satıldı - raporlama için</summary>
    Satildi = 4
}
