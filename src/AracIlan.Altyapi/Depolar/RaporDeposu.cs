using AracIlan.Alan.Sabitler;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Raporlama veri deposu - En çok ilan verilen marka/model vb.
/// </summary>
public class RaporDeposu(AracIlanVeritabani veritabani) : IRaporDeposu
{
    public async Task<IReadOnlyList<MarkaIlanSayisi>> EnCokIlanliMarkalarAsync(int adet, CancellationToken iptal = default)
    {
        return await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Yayinda)
            .GroupBy(a => new { a.MotorSecenegi.ModelPaketi.Model.MarkaId, a.MotorSecenegi.ModelPaketi.Model.Marka.Ad })
            .Select(g => new MarkaIlanSayisi(g.Key.MarkaId, g.Key.Ad, g.Count()))
            .OrderByDescending(x => x.IlanSayisi)
            .Take(adet)
            .ToListAsync(iptal);
    }

    public async Task<IReadOnlyList<ModelIlanSayisi>> EnCokIlanliModellerAsync(int adet, CancellationToken iptal = default)
    {
        return await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Yayinda)
            .GroupBy(a => new { a.MotorSecenegi.ModelPaketi.ModelId, ModelAd = a.MotorSecenegi.ModelPaketi.Model.Ad, MarkaAd = a.MotorSecenegi.ModelPaketi.Model.Marka.Ad })
            .Select(g => new ModelIlanSayisi(g.Key.ModelId, g.Key.ModelAd, g.Key.MarkaAd, g.Count()))
            .OrderByDescending(x => x.IlanSayisi)
            .Take(adet)
            .ToListAsync(iptal);
    }

    public async Task<IReadOnlyList<KategoriIlanSayisi>> KategoriBazliIlanSayilariAsync(CancellationToken iptal = default)
    {
        return await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Yayinda)
            .GroupBy(a => a.Kategori)
            .Select(g => new KategoriIlanSayisi(g.Key.ToString(), g.Count()))
            .ToListAsync(iptal);
    }

    public async Task<int> SonGunlerdeIlanSayisiAsync(int gun, CancellationToken iptal = default)
    {
        var tarih = DateTime.UtcNow.AddDays(-gun);
        return await veritabani.Araclar
            .CountAsync(a => a.IlanDurumu == IlanDurumu.Yayinda && a.OlusturmaTarihi >= tarih, iptal);
    }
}
