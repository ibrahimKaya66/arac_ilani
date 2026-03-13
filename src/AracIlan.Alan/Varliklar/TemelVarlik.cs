namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Tüm varlıklar için ortak alanlar (Base Entity).
/// Audit: kim ne zaman ekledi/güncelledi/sildi.
/// Soft delete: Silindi = true ile fiziksel silme yapılmaz.
/// </summary>
public abstract class TemelVarlik
{
    /// <summary>Birincil anahtar</summary>
    public int Id { get; set; }

    /// <summary>Kim ekledi (KullanıcıId veya sistem)</summary>
    public string? KimEklendi { get; set; }

    /// <summary>Ne zaman eklendi (UTC)</summary>
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

    /// <summary>Kim güncelledi (KullanıcıId)</summary>
    public string? KimGuncelledi { get; set; }

    /// <summary>Ne zaman güncellendi (UTC)</summary>
    public DateTime? GuncellemeTarihi { get; set; }

    /// <summary>Soft delete - Silindi mi?</summary>
    public bool Silindi { get; set; }

    /// <summary>Kim sildi (KullanıcıId - soft delete yapan)</summary>
    public string? KimSildi { get; set; }

    /// <summary>Ne zaman silindi (UTC - soft delete tarihi)</summary>
    public DateTime? SilinmeTarihi { get; set; }
}
