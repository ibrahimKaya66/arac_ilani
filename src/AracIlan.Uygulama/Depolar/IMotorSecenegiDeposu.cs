using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Motor seçeneği veri erişim arayüzü
/// Paket seçildiğinde motor seçenekleri getirilir
/// </summary>
public interface IMotorSecenegiDeposu
{
    Task<List<MotorSecenegi>> PaketIdIleGetirAsync(int modelPaketiId, CancellationToken iptal = default);
    Task<MotorSecenegi?> IdIleGetirAsync(int id, CancellationToken iptal = default);
}
