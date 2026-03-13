namespace AracIlan.Alan.Sabitler;

/// <summary>
/// Kullanıcı rolü - Dashboard ve yetkiler buna göre belirlenir
/// </summary>
public enum KullaniciRolu
{
    /// <summary>Standart üye - Sınırlı ilan hakkı</summary>
    Standart = 0,

    /// <summary>Premium üye - Daha fazla ilan ve fotoğraf</summary>
    Premium = 1,

    /// <summary>Bayi - Pakete göre özelleştirilmiş</summary>
    Bayi = 2,

    /// <summary>Yönetici - Tüm yetkiler</summary>
    Admin = 99
}
