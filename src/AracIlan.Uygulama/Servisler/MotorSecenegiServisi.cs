using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// Motor seçeneği iş mantığı - Paket seçildiğinde motorlar
/// </summary>
public class MotorSecenegiServisi
{
    private readonly IMotorSecenegiDeposu _motorDeposu;

    public MotorSecenegiServisi(IMotorSecenegiDeposu motorDeposu)
    {
        _motorDeposu = motorDeposu;
    }

    public async Task<List<MotorSecenegiYaniti>> PaketIdIleGetirAsync(int modelPaketiId, CancellationToken iptal = default)
    {
        var motorlar = await _motorDeposu.PaketIdIleGetirAsync(modelPaketiId, iptal);
        return motorlar.Select(m => new MotorSecenegiYaniti(
            m.Id, m.Ad, m.MotorHacmi, m.YakitTipi, m.Guc)).ToList();
    }
}
