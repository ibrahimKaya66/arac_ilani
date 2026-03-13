namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Tüm varlıklar için ortak alanlar.
/// Soft delete ve audit için kullanılır.
/// </summary>
public abstract class TemelVarlik
{
    /// <summary>Birincil anahtar</summary>
    public int Id { get; set; }

    /// <summary>Oluşturulma tarihi (UTC)</summary>
    public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

    /// <summary>Son güncelleme tarihi (UTC)</summary>
    public DateTime? GuncellemeTarihi { get; set; }

    /// <summary>Soft delete - Silindi mi?</summary>
    public bool Silindi { get; set; }
}
