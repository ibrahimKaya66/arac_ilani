namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Araç ilanına ait video
/// Fotoğraf gibi eklenip kaldırılabilir, kaldırıldığında dosya da silinir
/// </summary>
public class AracVideosu : TemelVarlik
{
    /// <summary>Dosya yolu veya URL</summary>
    public string DosyaYolu { get; set; } = string.Empty;

    /// <summary>Sıra</summary>
    public int Sira { get; set; }

    /// <summary>İlişkili araç</summary>
    public int AracId { get; set; }
    public Arac Arac { get; set; } = null!;
}
