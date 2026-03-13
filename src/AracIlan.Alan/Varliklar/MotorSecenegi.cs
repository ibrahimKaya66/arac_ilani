namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Motor seçeneği - Paket seçildiğinde listelenir.
/// AI bu seçeneğe göre teknik özellikleri dolduracak.
/// </summary>
public class MotorSecenegi : TemelVarlik
{
    /// <summary>Motor adı (örn: "1.6 TDI", "2.0 TFSI")</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Motor hacmi (cc) - 1600, 2000 vb.</summary>
    public int? MotorHacmi { get; set; }

    /// <summary>Yakıt tipi (Benzin, Dizel, Hibrit, Elektrik)</summary>
    public string YakitTipi { get; set; } = string.Empty;

    /// <summary>Güç (HP)</summary>
    public int? Guc { get; set; }

    /// <summary>İlişkili paket</summary>
    public int ModelPaketiId { get; set; }
    public ModelPaketi ModelPaketi { get; set; } = null!;
}
