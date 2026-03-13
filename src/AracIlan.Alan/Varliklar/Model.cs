namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Araç modeli (Corolla, 3 Serisi vb.)
/// Markaya bağlıdır.
/// </summary>
public class Model : TemelVarlik
{
    /// <summary>Model adı</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Model slug (URL için)</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Üretim başlangıç yılı</summary>
    public int UretimBaslangicYili { get; set; }

    /// <summary>Üretim bitiş yılı (devam ediyorsa gelecek yıl)</summary>
    public int UretimBitisYili { get; set; }

    /// <summary>İlişkili marka</summary>
    public int MarkaId { get; set; }
    public Marka Marka { get; set; } = null!;

    /// <summary>Bu modele ait paketler (yıl bazlı)</summary>
    public ICollection<ModelPaketi> Paketler { get; set; } = new List<ModelPaketi>();
}
