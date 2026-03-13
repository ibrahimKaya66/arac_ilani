using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using AracIlan.Sozlesmeler.Istekler;
using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// İlan iş mantığı - Oluşturma, listeleme, detay, ilan hakkı kontrolü
/// </summary>
public class IlanServisi(
    IAracDeposu aracDeposu,
    IMotorSecenegiDeposu motorDeposu,
    IKullaniciAbonelikDeposu abonelikDeposu,
    IAIServisi aiServisi) : IIlanServisi
{
    public async Task<IlanListeSayfaliYaniti> FiltreliGetirAsync(AracFiltre filtre, CancellationToken iptal = default)
    {
        var araclarTask = aracDeposu.FiltreliGetirAsync(filtre, iptal);
        var toplamTask = aracDeposu.FiltreliSayiAsync(filtre, iptal);
        await Task.WhenAll(araclarTask, toplamTask);

        var araclar = araclarTask.Result;
        var toplam = toplamTask.Result;

        var liste = araclar.Select(a => new IlanListeYaniti(
            a.Id,
            a.Baslik,
            a.UretimYili,
            a.Kilometre,
            a.Fiyat,
            a.Gorseller.FirstOrDefault()?.DosyaYolu ?? "",
            a.MotorSecenegi.ModelPaketi.Model.Marka.Ad,
            a.MotorSecenegi.ModelPaketi.Model.Ad,
            a.HasarDurumu.ToString()
        )).ToList();

        return new IlanListeSayfaliYaniti(liste, toplam, filtre.Sayfa, filtre.SayfaBoyutu);
    }

    public async Task<AracDetayYaniti?> DetayGetirAsync(int id, CancellationToken iptal = default)
    {
        var arac = await aracDeposu.IdVeDetaylarlaGetirAsync(id, iptal);
        if (arac == null) return null;

        return new AracDetayYaniti(
            arac.Id,
            arac.Baslik,
            arac.Aciklama,
            arac.Kategori.ToString(),
            arac.UretimYili,
            arac.Kilometre,
            arac.Fiyat,
            arac.Renk,
            arac.VitesTipi,
            arac.HasarDurumu.ToString(),
            arac.MotorSecenegi.ModelPaketi.Model.Marka.Ad,
            arac.MotorSecenegi.ModelPaketi.Model.Ad,
            arac.MotorSecenegi.Ad,
            [.. arac.Gorseller.OrderBy(g => g.Sira).Select(g => g.DosyaYolu)],
            arac.TeknikOzelliklerJson,
            arac.ExpertizRaporu?.AIAnalizSonucu,
            arac.IlanDurumu.ToString(),
            arac.KullaniciId
        );
    }

    /// <summary>
    /// Kullanıcının ilan hakkı var mı kontrol eder
    /// </summary>
    public async Task<(bool Yeterli, int KalanHak, int MaksFotograf)> IlanHakkiKontrolAsync(string kullaniciId, int fotografSayisi, CancellationToken iptal = default)
    {
        var (ilanHakki, maksFotograf, _) = await abonelikDeposu.AktifAbonelikGetirAsync(kullaniciId, iptal);
        var kullanilan = await abonelikDeposu.KullanilanIlanSayisiAsync(kullaniciId, iptal);
        var kalan = ilanHakki - kullanilan;

        if (kalan <= 0) return (false, 0, maksFotograf);
        if (fotografSayisi > maksFotograf) return (false, kalan, maksFotograf);

        return (true, kalan, maksFotograf);
    }

    public async Task<int> OlusturAsync(IlanOlusturmaIstegi istek, string kullaniciId, CancellationToken iptal = default)
    {
        var fotografSayisi = istek.GorselYollari?.Count ?? 0;
        var (yeterli, _, maksFotograf) = await IlanHakkiKontrolAsync(kullaniciId, fotografSayisi, iptal);
        if (!yeterli)
            throw new InvalidOperationException(fotografSayisi > maksFotograf
                ? $"Maksimum {maksFotograf} fotoğraf yükleyebilirsiniz"
                : "İlan hakkınız kalmadı. Üyelik paketi satın alın.");

        var motor = await motorDeposu.IdIleGetirAsync(istek.MotorSecenegiId, iptal);
        if (motor == null)
            throw new ArgumentException("Geçersiz motor seçeneği");

        var (_, _, ilanSuresiGun) = await abonelikDeposu.AktifAbonelikGetirAsync(kullaniciId, iptal);

        var baslik = $"{motor.ModelPaketi.Model.Marka.Ad} {motor.ModelPaketi.Model.Ad} {istek.UretimYili}";

        var marka = motor.ModelPaketi.Model.Marka.Ad;
        var modelAd = motor.ModelPaketi.Model.Ad;

        var teknikJson = await aiServisi.TeknikOzellikUretAsync(marka, modelAd, motor.Ad, motor.MotorHacmi, motor.YakitTipi, motor.Guc, istek.UretimYili, iptal);

        var arac = new Arac
        {
            Baslik = baslik,
            Aciklama = istek.Aciklama,
            Kategori = istek.Kategori,
            UretimYili = istek.UretimYili,
            Kilometre = istek.Kilometre,
            Fiyat = istek.Fiyat,
            Renk = istek.Renk,
            VitesTipi = istek.VitesTipi,
            HasarDurumu = istek.HasarDurumu,
            MotorSecenegiId = istek.MotorSecenegiId,
            KullaniciId = kullaniciId,
            IlanDurumu = IlanDurumu.Taslak,
            IlanBitisTarihi = DateTime.UtcNow.AddDays(ilanSuresiGun),
            TeknikOzelliklerJson = teknikJson
        };

        if (!string.IsNullOrEmpty(istek.ExpertizGorselYolu))
        {
            var expertizAnaliz = await aiServisi.ExpertizGorselAnalizAsync(istek.ExpertizGorselYolu, null, iptal);
            if (expertizAnaliz != null)
                arac.ExpertizRaporu = new ExpertizRaporu { GorselYolu = istek.ExpertizGorselYolu, AIAnalizSonucu = expertizAnaliz };
        }

        if (istek.GorselYollari is { } yollar)
        {
            var sira = 1;
            foreach (var yol in yollar)
                arac.Gorseller.Add(new AracGorseli { DosyaYolu = yol, Sira = sira++ });
        }

        var eklenen = await aracDeposu.EkleAsync(arac, iptal);
        return eklenen.Id;
    }

    public async Task<IlanListeSayfaliYaniti> KullaniciIlanlariniGetirAsync(string kullaniciId, int sayfa, int sayfaBoyutu, CancellationToken iptal = default)
    {
        var araclarTask = aracDeposu.KullaniciIlanlariniGetirAsync(kullaniciId, sayfa, sayfaBoyutu, iptal);
        var toplamTask = aracDeposu.KullaniciIlanSayisiAsync(kullaniciId, iptal);
        await Task.WhenAll(araclarTask, toplamTask);

        var araclar = araclarTask.Result;
        var toplam = toplamTask.Result;

        var liste = araclar.Select(a => new IlanListeYaniti(
            a.Id,
            a.Baslik,
            a.UretimYili,
            a.Kilometre,
            a.Fiyat,
            a.Gorseller.FirstOrDefault()?.DosyaYolu ?? "",
            a.MotorSecenegi.ModelPaketi.Model.Marka.Ad,
            a.MotorSecenegi.ModelPaketi.Model.Ad,
            a.HasarDurumu.ToString()
        )).ToList();

        return new IlanListeSayfaliYaniti(liste, toplam, sayfa, sayfaBoyutu);
    }

    public async Task<bool> YayinlaAsync(int ilanId, string kullaniciId, CancellationToken iptal = default)
    {
        var arac = await aracDeposu.IdIleGetirAsync(ilanId, iptal);
        if (arac == null || arac.KullaniciId != kullaniciId) return false;
        if (arac.IlanDurumu != IlanDurumu.Taslak) return false;

        arac.IlanDurumu = IlanDurumu.Yayinda;
        await aracDeposu.GuncelleAsync(arac, iptal);
        return true;
    }

    public async Task<bool> SatildiOlarakIsaretleAsync(int ilanId, string kullaniciId, CancellationToken iptal = default)
    {
        var arac = await aracDeposu.IdIleGetirAsync(ilanId, iptal);
        if (arac == null || arac.KullaniciId != kullaniciId) return false;
        if (arac.IlanDurumu != IlanDurumu.Yayinda) return false;

        arac.IlanDurumu = IlanDurumu.Satildi;
        arac.SatildiTarihi = DateTime.UtcNow;
        await aracDeposu.GuncelleAsync(arac, iptal);
        return true;
    }
}
