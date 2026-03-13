namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Expertiz raporu - AI ile analiz edilir.
/// Hangi parçalar orijinal, boyalı vb. bilgisi tutulur.
/// </summary>
public class ExpertizRaporu : TemelVarlik
{
    /// <summary>Expertiz görseli dosya yolu</summary>
    public string GorselYolu { get; set; } = string.Empty;

    /// <summary>AI analiz sonucu - Hangi parçalar değişmiş/boyalı (JSON)</summary>
    public string? AIAnalizSonucu { get; set; }

    /// <summary>İlişkili araç (1-1)</summary>
    public int AracId { get; set; }
    public Arac Arac { get; set; } = null!;
}
