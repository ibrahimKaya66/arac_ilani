using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Depolar;

/// <summary>
/// Kullanıcı abonelik veri deposu - İlan hakkı hesaplama
/// </summary>
public class KullaniciAbonelikDeposu(AracIlanVeritabani veritabani) : IKullaniciAbonelikDeposu
{
    public async Task<(int IlanHakki, int MaksimumFotograf, int IlanSuresiGun)> AktifAbonelikGetirAsync(string kullaniciId, CancellationToken iptal = default)
    {
        var simdi = DateTime.UtcNow;
        var abonelik = await veritabani.KullaniciAbonelikleri
            .Include(k => k.UyelikPaketi)
            .Where(k => k.KullaniciId == kullaniciId && k.BitisTarihi >= simdi)
            .OrderByDescending(k => k.BitisTarihi)
            .FirstOrDefaultAsync(iptal);

        if (abonelik != null)
            return (abonelik.UyelikPaketi.IlanHakki, abonelik.UyelikPaketi.MaksimumFotograf, abonelik.UyelikPaketi.IlanSuresiGun);

        // Varsayılan: Standart
        var standart = await veritabani.UyelikPaketleri
            .Where(u => u.Ad == "Standart")
            .FirstOrDefaultAsync(iptal);

        return standart != null
            ? (standart.IlanHakki, standart.MaksimumFotograf, standart.IlanSuresiGun)
            : (3, 8, 30);
    }

    public async Task<int> KullanilanIlanSayisiAsync(string kullaniciId, CancellationToken iptal = default)
    {
        return await veritabani.Araclar
            .CountAsync(a => a.KullaniciId == kullaniciId &&
                (a.IlanDurumu == IlanDurumu.Yayinda || a.IlanDurumu == IlanDurumu.Taslak), iptal);
    }
}
