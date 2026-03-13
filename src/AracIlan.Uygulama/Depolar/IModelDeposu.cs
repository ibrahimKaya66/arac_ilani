using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Model veri erişim arayüzü
/// </summary>
public interface IModelDeposu
{
    Task<List<Model>> MarkaIdIleGetirAsync(int markaId, CancellationToken iptal = default);
    Task<Model?> IdIleGetirAsync(int id, CancellationToken iptal = default);
}
