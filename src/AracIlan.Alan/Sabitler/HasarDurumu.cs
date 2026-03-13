namespace AracIlan.Alan.Sabitler;

/// <summary>
/// Araç hasar durumu. Expertiz girilmediğinde "Bilinmiyor" kabul edilir.
/// </summary>
public enum HasarDurumu
{
    /// <summary>Expertiz girilmemiş - Hasar durumu bilinmemektedir</summary>
    Bilinmiyor = 0,

    /// <summary>Hasarsız araç</summary>
    Hasarsiz = 1,

    /// <summary>Hasar kayıtlı araç</summary>
    HasarKayitli = 2,

    /// <summary>Expertiz raporu mevcut</summary>
    ExpertizVar = 3
}
