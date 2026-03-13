namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Araç ilanına ait fotoğraf
/// Üyelik paketine göre adet sınırı uygulanır
/// </summary>
public class AracGorseli : TemelVarlik
{
    /// <summary>Dosya yolu veya URL</summary>
    public string DosyaYolu { get; set; } = string.Empty;

    /// <summary>Sıra (1 = kapak fotoğrafı)</summary>
    public int Sira { get; set; }

    /// <summary>İlişkili araç</summary>
    public int AracId { get; set; }
    public Arac Arac { get; set; } = null!;
}
