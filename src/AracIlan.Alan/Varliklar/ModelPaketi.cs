namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Model paketi - Belirli yıl aralığındaki donanım paketi.
/// Kullanıcı yıl girdiğinde bu paketler listelenir.
/// </summary>
public class ModelPaketi : TemelVarlik
{
    /// <summary>Paket adı (örn: "Advance", "Confort")</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Geçerli olduğu başlangıç yılı</summary>
    public int BaslangicYili { get; set; }

    /// <summary>Geçerli olduğu bitiş yılı</summary>
    public int BitisYili { get; set; }

    /// <summary>İlişkili model</summary>
    public int ModelId { get; set; }
    public Model Model { get; set; } = null!;

    /// <summary>Bu pakete ait motor seçenekleri</summary>
    public ICollection<MotorSecenegi> MotorSecenekleri { get; set; } = new List<MotorSecenegi>();
}
