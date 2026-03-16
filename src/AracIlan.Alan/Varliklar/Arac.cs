using AracIlan.Alan.Sabitler;

namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Araç ilanı - Platformun ana varlığı
/// </summary>
public class Arac : TemelVarlik
{
    /// <summary>İlan başlığı (otomatik veya manuel)</summary>
    public string Baslik { get; set; } = string.Empty;

    /// <summary>İlan açıklaması</summary>
    public string Aciklama { get; set; } = string.Empty;

    /// <summary>Araç kategorisi (Otomobil, SUV, Pickup)</summary>
    public AracKategorisi Kategori { get; set; }

    /// <summary>Üretim yılı</summary>
    public int UretimYili { get; set; }

    /// <summary>Kilometre</summary>
    public int Kilometre { get; set; }

    /// <summary>Fiyat (TL)</summary>
    public decimal Fiyat { get; set; }

    /// <summary>Renk</summary>
    public string Renk { get; set; } = string.Empty;

    /// <summary>Vites tipi (Manuel, Otomatik, Yarı Otomatik)</summary>
    public string VitesTipi { get; set; } = string.Empty;

    /// <summary>Hasar durumu - Expertiz yoksa Bilinmiyor</summary>
    public HasarDurumu HasarDurumu { get; set; } = HasarDurumu.Bilinmiyor;

    /// <summary>İlan durumu</summary>
    public IlanDurumu IlanDurumu { get; set; } = IlanDurumu.Taslak;

    /// <summary>İlan bitiş tarihi (paket süresine göre)</summary>
    public DateTime? IlanBitisTarihi { get; set; }

    /// <summary>Satıldı tarihi - raporlama için</summary>
    public DateTime? SatildiTarihi { get; set; }

    /// <summary>İlan sahibi kullanıcı ID (Identity UserId - string)</summary>
    public string KullaniciId { get; set; } = string.Empty;

    /// <summary>Seçilen motor</summary>
    public int MotorSecenegiId { get; set; }
    public MotorSecenegi MotorSecenegi { get; set; } = null!;

    /// <summary>Teknik özellikler (AI ile doldurulur - JSON)</summary>
    public string? TeknikOzelliklerJson { get; set; }

    /// <summary>İlan fotoğrafları</summary>
    public ICollection<AracGorseli> Gorseller { get; set; } = new List<AracGorseli>();

    /// <summary>İlan videoları</summary>
    public ICollection<AracVideosu> Videolar { get; set; } = new List<AracVideosu>();

    /// <summary>Expertiz raporu (varsa)</summary>
    public ExpertizRaporu? ExpertizRaporu { get; set; }
}
