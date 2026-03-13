using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Model paketi veri erişim arayüzü
/// Yıl girildiğinde o yıla ait paketler getirilir
/// </summary>
public interface IModelPaketiDeposu
{
    Task<List<ModelPaketi>> ModelVeYilIleGetirAsync(int modelId, int yil, CancellationToken iptal = default);
    Task<ModelPaketi?> IdIleGetirAsync(int id, CancellationToken iptal = default);
}
