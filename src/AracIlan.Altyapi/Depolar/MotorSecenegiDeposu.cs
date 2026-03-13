using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Motor seçeneği veri deposu - Paket seçildiğinde motorlar
/// </summary>
public class MotorSecenegiDeposu : IMotorSecenegiDeposu
{
    private readonly AracIlanVeritabani _veritabani;

    public MotorSecenegiDeposu(AracIlanVeritabani veritabani)
    {
        _veritabani = veritabani;
    }

    public async Task<List<MotorSecenegi>> PaketIdIleGetirAsync(int modelPaketiId, CancellationToken iptal = default)
    {
        return await _veritabani.MotorSecenekleri
            .Where(m => m.ModelPaketiId == modelPaketiId)
            .OrderBy(m => m.Ad)
            .ToListAsync(iptal);
    }

    public async Task<MotorSecenegi?> IdIleGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.MotorSecenekleri
            .Include(m => m.ModelPaketi).ThenInclude(p => p!.Model).ThenInclude(m => m!.Marka)
            .FirstOrDefaultAsync(m => m.Id == id, iptal);
    }
}
