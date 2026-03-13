using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Marka veri erişim arayüzü
/// </summary>
public interface IMarkaDeposu
{
    Task<List<Marka>> KategoriyeGoreGetirAsync(AracKategorisi kategori, CancellationToken iptal = default);
    Task<Marka?> IdIleGetirAsync(int id, CancellationToken iptal = default);
}
