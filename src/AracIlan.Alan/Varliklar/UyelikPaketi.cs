namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Üyelik paketi - Uygulama içi satın alım ile alınır.
/// İlan adedi, fotoğraf sayısı, süre burada tanımlı.
/// </summary>
public class UyelikPaketi : TemelVarlik
{
    /// <summary>Paket adı (Standart, Premium, Bayi Pro vb.)</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Açıklama</summary>
    public string Aciklama { get; set; } = string.Empty;

    /// <summary>Fiyat (TL)</summary>
    public decimal Fiyat { get; set; }

    /// <summary>İlan hakkı adedi</summary>
    public int IlanHakki { get; set; }

    /// <summary>İlan başına maksimum fotoğraf</summary>
    public int MaksimumFotograf { get; set; }

    /// <summary>İlan süresi (gün)</summary>
    public int IlanSuresiGun { get; set; }

    /// <summary>Sıra (listeleme için)</summary>
    public int Sira { get; set; }

    /// <summary>Aktif mi?</summary>
    public bool Aktif { get; set; } = true;
}
