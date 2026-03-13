using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AracIlan.Altyapi.Veritabani;

/// <summary>
/// İlk veri tohumu - Roller, marka/model, üyelik paketleri
/// </summary>
public static class VeriTohumu
{
    public static async Task TohumlaAsync(AracIlanVeritabani db, IServiceProvider servisler, CancellationToken iptal = default)
    {
        await RolleriTohumlaAsync(servisler, iptal);
        if (await db.Markalar.AnyAsync(iptal)) return;

        var toyota = new Marka { Ad = "Toyota", Slug = "toyota", Sira = 1, Kategori = AracKategorisi.Otomobil };
        db.Markalar.Add(toyota);
        await db.SaveChangesAsync(iptal);

        var corolla = new Model { Ad = "Corolla", Slug = "corolla", MarkaId = toyota.Id, UretimBaslangicYili = 2018, UretimBitisYili = 2024 };
        db.Modeller.Add(corolla);
        await db.SaveChangesAsync(iptal);

        var corollaAdvance = new ModelPaketi { Ad = "Advance", ModelId = corolla.Id, BaslangicYili = 2020, BitisYili = 2024 };
        db.ModelPaketleri.Add(corollaAdvance);
        await db.SaveChangesAsync(iptal);

        db.MotorSecenekleri.AddRange(
            new MotorSecenegi { Ad = "1.6 Hybrid", MotorHacmi = 1600, YakitTipi = "Hibrit", Guc = 122, ModelPaketiId = corollaAdvance.Id },
            new MotorSecenegi { Ad = "1.8 Hybrid", MotorHacmi = 1800, YakitTipi = "Hibrit", Guc = 140, ModelPaketiId = corollaAdvance.Id }
        );

        var bmw = new Marka { Ad = "BMW", Slug = "bmw", Sira = 2, Kategori = AracKategorisi.Otomobil };
        db.Markalar.Add(bmw);
        await db.SaveChangesAsync(iptal);

        var seri3 = new Model { Ad = "3 Serisi", Slug = "3-serisi", MarkaId = bmw.Id, UretimBaslangicYili = 2019, UretimBitisYili = 2024 };
        db.Modeller.Add(seri3);
        await db.SaveChangesAsync(iptal);

        var seri3Paket = new ModelPaketi { Ad = "M Sport", ModelId = seri3.Id, BaslangicYili = 2019, BitisYili = 2024 };
        db.ModelPaketleri.Add(seri3Paket);
        await db.SaveChangesAsync(iptal);

        db.MotorSecenekleri.AddRange(
            new MotorSecenegi { Ad = "320i", MotorHacmi = 2000, YakitTipi = "Benzin", Guc = 184, ModelPaketiId = seri3Paket.Id },
            new MotorSecenegi { Ad = "330i", MotorHacmi = 2000, YakitTipi = "Benzin", Guc = 258, ModelPaketiId = seri3Paket.Id }
        );

        db.UyelikPaketleri.AddRange(
            new UyelikPaketi { Ad = "Standart", Aciklama = "3 ilan, 8 fotoğraf, 30 gün", Fiyat = 0, IlanHakki = 3, MaksimumFotograf = 8, IlanSuresiGun = 30, Sira = 1 },
            new UyelikPaketi { Ad = "Premium", Aciklama = "10 ilan, 24 fotoğraf, 60 gün", Fiyat = 199, IlanHakki = 10, MaksimumFotograf = 24, IlanSuresiGun = 60, Sira = 2 }
        );

        await db.SaveChangesAsync(iptal);
    }

    private static async Task RolleriTohumlaAsync(IServiceProvider servisler, CancellationToken iptal)
    {
        var roleManager = servisler.GetRequiredService<RoleManager<IdentityRole>>();
        var roller = new[] { "Standart", "Premium", "Bayi", "Admin" };

        foreach (var rol in roller)
        {
            if (await roleManager.RoleExistsAsync(rol)) continue;
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }
}
