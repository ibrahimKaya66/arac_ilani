namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// AI servisi - Teknik özellik üretimi ve expertiz görsel analizi
/// OpenAI GPT-4 / Vision API kullanır
/// </summary>
public interface IAIServisi
{
    /// <summary>
    /// Motor bilgisine göre teknik özellikleri JSON olarak üretir
    /// </summary>
    /// <param name="markaAd">Marka adı</param>
    /// <param name="modelAd">Model adı</param>
    /// <param name="motorAd">Motor adı</param>
    /// <param name="motorHacmi">Motor hacmi (cc)</param>
    /// <param name="yakitTipi">Yakıt tipi</param>
    /// <param name="guc">Güç (HP)</param>
    /// <param name="uretimYili">Üretim yılı</param>
    Task<string?> TeknikOzellikUretAsync(string markaAd, string modelAd, string motorAd, int? motorHacmi, string yakitTipi, int? guc, int uretimYili, CancellationToken iptal = default);

    /// <summary>
    /// Expertiz görselini analiz eder - Hangi parçalar orijinal, boyalı, değişmiş
    /// </summary>
    /// <param name="gorselYolu">Dosya yolu veya base64</param>
    /// <param name="gorselStream">Görsel stream (dosya yolu yoksa)</param>
    Task<string?> ExpertizGorselAnalizAsync(string? gorselYolu, Stream? gorselStream, CancellationToken iptal = default);
}
