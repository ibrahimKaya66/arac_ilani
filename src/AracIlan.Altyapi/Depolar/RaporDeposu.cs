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

    public async Task<TarihAraligiRaporu> TarihAraligiSatisRaporuAsync(DateTime baslangic, DateTime bitis, CancellationToken iptal = default)
    {
        var satislar = await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Satildi && a.SatildiTarihi >= baslangic && a.SatildiTarihi <= bitis)
            .ToListAsync(iptal);
        var toplamSatis = satislar.Count;
        var toplamCiro = satislar.Sum(a => a.Fiyat);
        var aktifIlan = await veritabani.Araclar.CountAsync(a => a.IlanDurumu == IlanDurumu.Yayinda, iptal);
        return new TarihAraligiRaporu(toplamSatis, toplamCiro, aktifIlan);
    }

    public async Task<IReadOnlyList<YilBazliSatisRaporu>> UretimYilinaGoreSatisAsync(DateTime baslangic, DateTime bitis, CancellationToken iptal = default)
    {
        return await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Satildi && a.SatildiTarihi >= baslangic && a.SatildiTarihi <= bitis)
            .GroupBy(a => a.UretimYili)
            .Select(g => new YilBazliSatisRaporu(g.Key, g.Count(), g.Average(a => a.Fiyat)))
            .OrderByDescending(x => x.SatisAdedi)
            .ToListAsync(iptal);
    }

    public async Task<IReadOnlyList<HizliSatisRaporu>> EnHizliSatilanlarAsync(int adet, CancellationToken iptal = default)
    {
        var satislar = await veritabani.Araclar
            .Where(a => a.IlanDurumu == IlanDurumu.Satildi && a.SatildiTarihi != null)
            .Select(a => new { a.Id, a.Baslik, a.OlusturmaTarihi, a.SatildiTarihi, a.Fiyat })
            .ToListAsync(iptal);
        return satislar
            .Select(a => new HizliSatisRaporu(a.Id, a.Baslik, (int)(a.SatildiTarihi!.Value - a.OlusturmaTarihi).TotalDays, a.Fiyat))
            .OrderBy(x => x.GunIcindeSatildi)
            .Take(adet)
            .ToList();
    }
}
