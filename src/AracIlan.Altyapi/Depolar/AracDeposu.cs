using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Araç ilanı veri deposu implementasyonu
/// </summary>
public class AracDeposu : IAracDeposu
{
    private readonly AracIlanVeritabani _veritabani;

    public AracDeposu(AracIlanVeritabani veritabani)
    {
        _veritabani = veritabani;
    }

    public async Task<Arac?> IdIleGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.Araclar.FindAsync([id], iptal);
    }

    public async Task<Arac?> IdVeDetaylarlaGetirAsync(int id, CancellationToken iptal = default)
    {
        return await _veritabani.Araclar
            .Include(a => a.MotorSecenegi)
                .ThenInclude(m => m!.ModelPaketi)
                    .ThenInclude(p => p!.Model)
                        .ThenInclude(m => m!.Marka)
            .Include(a => a.Gorseller.OrderBy(g => g.Sira))
            .Include(a => a.Videolar.OrderBy(v => v.Sira))
            .Include(a => a.ExpertizRaporu)
            .FirstOrDefaultAsync(a => a.Id == id, iptal);
    }

    public async Task<List<Arac>> FiltreliGetirAsync(AracFiltre filtre, CancellationToken iptal = default)
    {
        var sorgu = _veritabani.Araclar
            .Include(a => a.MotorSecenegi)
                .ThenInclude(m => m!.ModelPaketi)
                    .ThenInclude(p => p!.Model)
                        .ThenInclude(m => m!.Marka)
            .Include(a => a.Gorseller.OrderBy(g => g.Sira).Take(1)) // Kapak fotoğrafı
            .Where(a => a.IlanDurumu == IlanDurumu.Yayinda);

        if (filtre.Kategori.HasValue)
            sorgu = sorgu.Where(a => a.Kategori == filtre.Kategori.Value);
        if (filtre.MarkaId.HasValue)
            sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.Model.MarkaId == filtre.MarkaId.Value);
        if (filtre.ModelId.HasValue)
            sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.ModelId == filtre.ModelId.Value);
        if (filtre.MinYil.HasValue)
            sorgu = sorgu.Where(a => a.UretimYili >= filtre.MinYil.Value);
        if (filtre.MaxYil.HasValue)
            sorgu = sorgu.Where(a => a.UretimYili <= filtre.MaxYil.Value);
        if (filtre.MinFiyat.HasValue)
            sorgu = sorgu.Where(a => a.Fiyat >= filtre.MinFiyat.Value);
        if (filtre.MaxFiyat.HasValue)
            sorgu = sorgu.Where(a => a.Fiyat <= filtre.MaxFiyat.Value);
        if (filtre.MinKm.HasValue)
            sorgu = sorgu.Where(a => a.Kilometre >= filtre.MinKm.Value);
        if (filtre.MaxKm.HasValue)
            sorgu = sorgu.Where(a => a.Kilometre <= filtre.MaxKm.Value);

        sorgu = filtre.Siralama?.ToLowerInvariant() switch
        {
            "fiyat" => sorgu.OrderBy(a => a.Fiyat),
            "fiyat_desc" => sorgu.OrderByDescending(a => a.Fiyat),
            "yil" => sorgu.OrderBy(a => a.UretimYili),
            "yil_desc" => sorgu.OrderByDescending(a => a.UretimYili),
            "km" => sorgu.OrderBy(a => a.Kilometre),
            "km_desc" => sorgu.OrderByDescending(a => a.Kilometre),
            _ => sorgu.OrderByDescending(a => a.OlusturmaTarihi)
        };

        var atla = (filtre.Sayfa - 1) * filtre.SayfaBoyutu;
        return await sorgu
            .Skip(atla)
            .Take(filtre.SayfaBoyutu)
            .ToListAsync(iptal);
    }

    public async Task<int> FiltreliSayiAsync(AracFiltre filtre, CancellationToken iptal = default)
    {
        var sorgu = _veritabani.Araclar.Where(a => a.IlanDurumu == IlanDurumu.Yayinda);

        if (filtre.Kategori.HasValue) sorgu = sorgu.Where(a => a.Kategori == filtre.Kategori.Value);
        if (filtre.MarkaId.HasValue) sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.Model.MarkaId == filtre.MarkaId.Value);
        if (filtre.ModelId.HasValue) sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.ModelId == filtre.ModelId.Value);
        if (filtre.MinYil.HasValue) sorgu = sorgu.Where(a => a.UretimYili >= filtre.MinYil.Value);
        if (filtre.MaxYil.HasValue) sorgu = sorgu.Where(a => a.UretimYili <= filtre.MaxYil.Value);
        if (filtre.MinFiyat.HasValue) sorgu = sorgu.Where(a => a.Fiyat >= filtre.MinFiyat.Value);
        if (filtre.MaxFiyat.HasValue) sorgu = sorgu.Where(a => a.Fiyat <= filtre.MaxFiyat.Value);
        if (filtre.MinKm.HasValue) sorgu = sorgu.Where(a => a.Kilometre >= filtre.MinKm.Value);
        if (filtre.MaxKm.HasValue) sorgu = sorgu.Where(a => a.Kilometre <= filtre.MaxKm.Value);

        return await sorgu.CountAsync(iptal);
    }

    public async Task<List<Arac>> KullaniciIlanlariniGetirAsync(string kullaniciId, int sayfa, int sayfaBoyutu, CancellationToken iptal = default)
    {
        var atla = (sayfa - 1) * sayfaBoyutu;
        return await _veritabani.Araclar
            .Include(a => a.MotorSecenegi)
                .ThenInclude(m => m!.ModelPaketi)
                    .ThenInclude(p => p!.Model)
                        .ThenInclude(m => m!.Marka)
            .Include(a => a.Gorseller.OrderBy(g => g.Sira).Take(1))
            .Where(a => a.KullaniciId == kullaniciId)
            .OrderByDescending(a => a.OlusturmaTarihi)
            .Skip(atla)
            .Take(sayfaBoyutu)
            .ToListAsync(iptal);
    }

    public async Task<int> KullaniciIlanSayisiAsync(string kullaniciId, CancellationToken iptal = default)
    {
        return await _veritabani.Araclar.CountAsync(a => a.KullaniciId == kullaniciId, iptal);
    }

    public async Task<List<Arac>> KullaniciIlanlariniFiltreliGetirAsync(string kullaniciId, AracFiltre filtre, CancellationToken iptal = default)
    {
        var sorgu = _veritabani.Araclar
            .Include(a => a.MotorSecenegi)
                .ThenInclude(m => m!.ModelPaketi)
                    .ThenInclude(p => p!.Model)
                        .ThenInclude(m => m!.Marka)
            .Include(a => a.Gorseller.OrderBy(g => g.Sira).Take(1))
            .Where(a => a.KullaniciId == kullaniciId);

        if (filtre.Kategori.HasValue)
            sorgu = sorgu.Where(a => a.Kategori == filtre.Kategori.Value);
        if (filtre.MarkaId.HasValue)
            sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.Model.MarkaId == filtre.MarkaId.Value);
        if (filtre.ModelId.HasValue)
            sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.ModelId == filtre.ModelId.Value);
        if (filtre.MinYil.HasValue)
            sorgu = sorgu.Where(a => a.UretimYili >= filtre.MinYil.Value);
        if (filtre.MaxYil.HasValue)
            sorgu = sorgu.Where(a => a.UretimYili <= filtre.MaxYil.Value);
        if (filtre.MinFiyat.HasValue)
            sorgu = sorgu.Where(a => a.Fiyat >= filtre.MinFiyat.Value);
        if (filtre.MaxFiyat.HasValue)
            sorgu = sorgu.Where(a => a.Fiyat <= filtre.MaxFiyat.Value);
        if (filtre.MinKm.HasValue)
            sorgu = sorgu.Where(a => a.Kilometre >= filtre.MinKm.Value);
        if (filtre.MaxKm.HasValue)
            sorgu = sorgu.Where(a => a.Kilometre <= filtre.MaxKm.Value);

        sorgu = filtre.Siralama?.ToLowerInvariant() switch
        {
            "fiyat" => sorgu.OrderBy(a => a.Fiyat),
            "fiyat_desc" => sorgu.OrderByDescending(a => a.Fiyat),
            "yil" => sorgu.OrderBy(a => a.UretimYili),
            "yil_desc" => sorgu.OrderByDescending(a => a.UretimYili),
            "km" => sorgu.OrderBy(a => a.Kilometre),
            "km_desc" => sorgu.OrderByDescending(a => a.Kilometre),
            _ => sorgu.OrderByDescending(a => a.OlusturmaTarihi)
        };

        var atla = (filtre.Sayfa - 1) * filtre.SayfaBoyutu;
        return await sorgu.Skip(atla).Take(filtre.SayfaBoyutu).ToListAsync(iptal);
    }

    public async Task<int> KullaniciIlanFiltreliSayiAsync(string kullaniciId, AracFiltre filtre, CancellationToken iptal = default)
    {
        var sorgu = _veritabani.Araclar.Where(a => a.KullaniciId == kullaniciId);
        if (filtre.Kategori.HasValue) sorgu = sorgu.Where(a => a.Kategori == filtre.Kategori.Value);
        if (filtre.MarkaId.HasValue) sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.Model.MarkaId == filtre.MarkaId.Value);
        if (filtre.ModelId.HasValue) sorgu = sorgu.Where(a => a.MotorSecenegi.ModelPaketi.ModelId == filtre.ModelId.Value);
        if (filtre.MinYil.HasValue) sorgu = sorgu.Where(a => a.UretimYili >= filtre.MinYil.Value);
        if (filtre.MaxYil.HasValue) sorgu = sorgu.Where(a => a.UretimYili <= filtre.MaxYil.Value);
        if (filtre.MinFiyat.HasValue) sorgu = sorgu.Where(a => a.Fiyat >= filtre.MinFiyat.Value);
        if (filtre.MaxFiyat.HasValue) sorgu = sorgu.Where(a => a.Fiyat <= filtre.MaxFiyat.Value);
        if (filtre.MinKm.HasValue) sorgu = sorgu.Where(a => a.Kilometre >= filtre.MinKm.Value);
        if (filtre.MaxKm.HasValue) sorgu = sorgu.Where(a => a.Kilometre <= filtre.MaxKm.Value);
        return await sorgu.CountAsync(iptal);
    }

    public async Task<bool> SilAsync(int id, string kullaniciId, CancellationToken iptal = default)
    {
        var arac = await _veritabani.Araclar
            .FirstOrDefaultAsync(a => a.Id == id && a.KullaniciId == kullaniciId, iptal);
        if (arac == null) return false;

        arac.Silindi = true;
        arac.KimSildi = kullaniciId;
        arac.SilinmeTarihi = DateTime.UtcNow;
        await _veritabani.SaveChangesAsync(iptal);
        return true;
    }

    public async Task<Arac> EkleAsync(Arac arac, CancellationToken iptal = default)
    {
        _veritabani.Araclar.Add(arac);
        await _veritabani.SaveChangesAsync(iptal);
        return arac;
    }

    public async Task GuncelleAsync(Arac arac, CancellationToken iptal = default)
    {
        _veritabani.Araclar.Update(arac);
        await _veritabani.SaveChangesAsync(iptal);
    }
}
