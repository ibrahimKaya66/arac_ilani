namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Kullanıcının satın aldığı üyelik paketi
/// </summary>
public class KullaniciAboneligi : TemelVarlik
{
    /// <summary>Kullanıcı ID (Identity)</summary>
    public string KullaniciId { get; set; } = string.Empty;

    /// <summary>Satın alınan paket</summary>
    public int UyelikPaketiId { get; set; }
    public UyelikPaketi UyelikPaketi { get; set; } = null!;

    /// <summary>Başlangıç tarihi</summary>
    public DateTime BaslangicTarihi { get; set; }

    /// <summary>Bitiş tarihi</summary>
    public DateTime BitisTarihi { get; set; }

    /// <summary>Kullanılan ilan hakkı</summary>
    public int KullanilanIlanHakki { get; set; }

    /// <summary>Ödeme referansı (Iyzico vb.)</summary>
    public string? OdemeReferansi { get; set; }
}
