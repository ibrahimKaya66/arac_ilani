namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// Dosya yükleme servisi - Araç fotoğrafları, expertiz
/// </summary>
public interface IDosyaServisi
{
    /// <summary>Dosyayı kaydeder, döndürülen yol ile ilan oluşturulur</summary>
    Task<string> AraçGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default);

    /// <summary>Expertiz görseli kaydeder</summary>
    Task<string> ExpertizGorseliKaydetAsync(Stream dosya, string dosyaAdi, string kullaniciId, CancellationToken iptal = default);

    /// <summary>Uploads klasöründeki dosyayı siler (örn. /uploads/araclar/xxx.jpg)</summary>
    void DosyaSil(string gorselYolu);
}
