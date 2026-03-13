using AracIlan.Alan.Sabitler;

namespace AracIlan.Alan.Varliklar;

/// <summary>
/// Araç markası (Toyota, BMW, Mercedes vb.)
/// </summary>
public class Marka : TemelVarlik
{
    /// <summary>Marka adı</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Marka slug (URL için)</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Sıralama (küçük = önce)</summary>
    public int Sira { get; set; }

    /// <summary>Bu markanın hangi kategoride olduğu (her marka tüm kategorilerde olabilir)</summary>
    public AracKategorisi Kategori { get; set; }

    /// <summary>Bu markaya ait modeller</summary>
    public ICollection<Model> Modeller { get; set; } = new List<Model>();
}
