using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AracIlan.Altyapi.Veritabani;

/// <summary>
/// İlk veri tohumu - Roller, marka/model, üyelik paketleri
/// Tüm yıllar 2020+ - Türkiye pazarına uygun zengin marka-model verisi
/// </summary>
public static class VeriTohumu
{
    private const int YilBaslangic = 2020;
    private static int YilBitis => DateTime.UtcNow.Year + 1;

    public static async Task TohumlaAsync(AracIlanVeritabani db, IServiceProvider servisler, CancellationToken iptal = default)
    {
        await RolleriTohumlaAsync(servisler, iptal);
        if (await db.Markalar.AnyAsync(iptal)) return;

        await UyelikPaketleriniTohumlaAsync(db, iptal);
        await TumMarkaVeModelleriTohumlaAsync(db, iptal);
    }

    /// <summary>
    /// Eksik kategorilere marka/model ekler. Kategori boşsa tam doldurur, kısmen doluysa eksikleri ekler.
    /// Tüm yıllar 2020+
    /// </summary>
    public static async Task TohumlaKategoriEksikseAsync(AracIlanVeritabani db, IServiceProvider servisler, AracKategorisi kategori, CancellationToken iptal = default)
    {
        await UyelikPaketleriniTohumlaAsync(db, iptal);

        var (tumMarkalar, modellerData) = KategoriMarkaModelData(kategori);
        if (tumMarkalar.Count == 0) return;

        var mevcutAdlar = await db.Markalar.Where(m => m.Kategori == kategori).Select(m => m.Ad).ToListAsync(iptal);
        var eklenecekMarkalar = tumMarkalar.Where(m => !mevcutAdlar.Contains(m.Ad)).ToList();
        if (eklenecekMarkalar.Count == 0) return;

        db.Markalar.AddRange(eklenecekMarkalar);
        await db.SaveChangesAsync(iptal);

        var modeller = new List<Model>();
        foreach (var m in eklenecekMarkalar)
        {
            var modellerMarka = modellerData.GetValueOrDefault(m.Ad, new[] { ("Model", YilBaslangic, YilBitis) });
            foreach (var (ad, baslangic, bitis) in modellerMarka)
            {
                modeller.Add(new Model { Ad = ad, Slug = ad.ToLowerInvariant().Replace(" ", "-"), MarkaId = m.Id, UretimBaslangicYili = baslangic, UretimBitisYili = bitis });
            }
        }
        db.Modeller.AddRange(modeller);
        await db.SaveChangesAsync(iptal);

        foreach (var model in modeller)
        {
            var paket = new ModelPaketi { Ad = "Standart", ModelId = model.Id, BaslangicYili = model.UretimBaslangicYili, BitisYili = model.UretimBitisYili };
            db.ModelPaketleri.Add(paket);
            await db.SaveChangesAsync(iptal);
            db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.0 - 1.6", MotorHacmi = 1400, YakitTipi = "Benzin", Guc = 100, ModelPaketiId = paket.Id });
            db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.0 - 1.6 Turbo", MotorHacmi = 1400, YakitTipi = "Benzin", Guc = 130, ModelPaketiId = paket.Id });
            db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.6 - 2.0", MotorHacmi = 1800, YakitTipi = "Benzin", Guc = 150, ModelPaketiId = paket.Id });
            db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "Dizel", MotorHacmi = 1600, YakitTipi = "Dizel", Guc = 120, ModelPaketiId = paket.Id });
            db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "Hibrit", MotorHacmi = 1800, YakitTipi = "Hibrit", Guc = 140, ModelPaketiId = paket.Id });
        }
        await db.SaveChangesAsync(iptal);
    }

    private static async Task UyelikPaketleriniTohumlaAsync(AracIlanVeritabani db, CancellationToken iptal)
    {
        if (await db.UyelikPaketleri.AnyAsync(iptal)) return;
        db.UyelikPaketleri.AddRange(
            new UyelikPaketi { Ad = "Standart", Aciklama = "3 ilan, 8 fotoğraf, 30 gün", Fiyat = 0, IlanHakki = 3, MaksimumFotograf = 8, IlanSuresiGun = 30, Sira = 1 },
            new UyelikPaketi { Ad = "Premium", Aciklama = "10 ilan, 24 fotoğraf, 60 gün", Fiyat = 199, IlanHakki = 10, MaksimumFotograf = 24, IlanSuresiGun = 60, Sira = 2 }
        );
        await db.SaveChangesAsync(iptal);
    }

    private static async Task TumMarkaVeModelleriTohumlaAsync(AracIlanVeritabani db, CancellationToken iptal)
    {
        foreach (var kategori in new[] { AracKategorisi.Otomobil, AracKategorisi.SUV, AracKategorisi.Pickup })
        {
            if (await db.Markalar.AnyAsync(m => m.Kategori == kategori, iptal)) continue;
            var (markalar, modellerData) = KategoriMarkaModelData(kategori);
            if (markalar.Count == 0) continue;

            db.Markalar.AddRange(markalar);
            await db.SaveChangesAsync(iptal);

            var modeller = new List<Model>();
            foreach (var m in markalar)
            {
                var modellerMarka = modellerData.GetValueOrDefault(m.Ad, new[] { ("Model", YilBaslangic, YilBitis) });
                foreach (var (ad, baslangic, bitis) in modellerMarka)
                {
                    modeller.Add(new Model { Ad = ad, Slug = ad.ToLowerInvariant().Replace(" ", "-"), MarkaId = m.Id, UretimBaslangicYili = baslangic, UretimBitisYili = bitis });
                }
            }
            db.Modeller.AddRange(modeller);
            await db.SaveChangesAsync(iptal);

            foreach (var model in modeller)
            {
                var paket = new ModelPaketi { Ad = "Standart", ModelId = model.Id, BaslangicYili = model.UretimBaslangicYili, BitisYili = model.UretimBitisYili };
                db.ModelPaketleri.Add(paket);
                await db.SaveChangesAsync(iptal);
                db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.0 - 1.6", MotorHacmi = 1400, YakitTipi = "Benzin", Guc = 100, ModelPaketiId = paket.Id });
                db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.0 - 1.6 Turbo", MotorHacmi = 1400, YakitTipi = "Benzin", Guc = 130, ModelPaketiId = paket.Id });
                db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "1.6 - 2.0", MotorHacmi = 1800, YakitTipi = "Benzin", Guc = 150, ModelPaketiId = paket.Id });
                db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "Dizel", MotorHacmi = 1600, YakitTipi = "Dizel", Guc = 120, ModelPaketiId = paket.Id });
                db.MotorSecenekleri.Add(new MotorSecenegi { Ad = "Hibrit", MotorHacmi = 1800, YakitTipi = "Hibrit", Guc = 140, ModelPaketiId = paket.Id });
            }
            await db.SaveChangesAsync(iptal);
        }
    }

    private static (List<Marka> Markalar, Dictionary<string, (string Ad, int Baslangic, int Bitis)[]> ModellerData) KategoriMarkaModelData(AracKategorisi kategori)
    {
        var sira = 1;
        if (kategori == AracKategorisi.Otomobil)
        {
            var markalar = new List<Marka>
            {
                new() { Ad = "Toyota", Slug = "toyota", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Honda", Slug = "honda", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Volkswagen", Slug = "volkswagen", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Renault", Slug = "renault", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Fiat", Slug = "fiat", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Ford", Slug = "ford", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Hyundai", Slug = "hyundai", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Kia", Slug = "kia", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "BMW", Slug = "bmw", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Mercedes-Benz", Slug = "mercedes-benz", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Audi", Slug = "audi", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Nissan", Slug = "nissan", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Peugeot", Slug = "peugeot", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Citroën", Slug = "citroen", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Opel", Slug = "opel", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Dacia", Slug = "dacia", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Mazda", Slug = "mazda", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Skoda", Slug = "skoda", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Seat", Slug = "seat", Sira = sira++, Kategori = AracKategorisi.Otomobil },
                new() { Ad = "Togg", Slug = "togg", Sira = sira++, Kategori = AracKategorisi.Otomobil },
            };
            var modeller = new Dictionary<string, (string, int, int)[]>
            {
                ["Toyota"] = new[] { ("Corolla", YilBaslangic, YilBitis), ("Yaris", YilBaslangic, YilBitis), ("Camry", YilBaslangic, YilBitis), ("C-HR", YilBaslangic, YilBitis) },
                ["Honda"] = new[] { ("Civic", YilBaslangic, YilBitis), ("Jazz", YilBaslangic, YilBitis), ("Accord", YilBaslangic, YilBitis) },
                ["Volkswagen"] = new[] { ("Golf", YilBaslangic, YilBitis), ("Polo", YilBaslangic, YilBitis), ("Passat", YilBaslangic, YilBitis), ("Jetta", YilBaslangic, YilBitis) },
                ["Renault"] = new[] { ("Clio", YilBaslangic, YilBitis), ("Megane", YilBaslangic, YilBitis), ("Talisman", YilBaslangic, YilBitis) },
                ["Fiat"] = new[] { ("Egea", YilBaslangic, YilBitis), ("Tipo", YilBaslangic, YilBitis) },
                ["Ford"] = new[] { ("Focus", YilBaslangic, YilBitis), ("Fiesta", YilBaslangic, YilBitis), ("Mondeo", YilBaslangic, YilBitis) },
                ["Hyundai"] = new[] { ("i20", YilBaslangic, YilBitis), ("i30", YilBaslangic, YilBitis), ("Elantra", YilBaslangic, YilBitis) },
                ["Kia"] = new[] { ("Ceed", YilBaslangic, YilBitis), ("Rio", YilBaslangic, YilBitis), ("Cerato", YilBaslangic, YilBitis) },
                ["BMW"] = new[] { ("3 Serisi", YilBaslangic, YilBitis), ("5 Serisi", YilBaslangic, YilBitis), ("1 Serisi", YilBaslangic, YilBitis) },
                ["Mercedes-Benz"] = new[] { ("C Serisi", YilBaslangic, YilBitis), ("E Serisi", YilBaslangic, YilBitis), ("A Serisi", YilBaslangic, YilBitis) },
                ["Audi"] = new[] { ("A3", YilBaslangic, YilBitis), ("A4", YilBaslangic, YilBitis), ("A6", YilBaslangic, YilBitis) },
                ["Nissan"] = new[] { ("Qashqai", YilBaslangic, YilBitis), ("Juke", YilBaslangic, YilBitis), ("Leaf", YilBaslangic, YilBitis), ("Micra", YilBaslangic, YilBitis) },
                ["Peugeot"] = new[] { ("208", YilBaslangic, YilBitis), ("308", YilBaslangic, YilBitis), ("508", YilBaslangic, YilBitis) },
                ["Citroën"] = new[] { ("C3", YilBaslangic, YilBitis), ("C4", YilBaslangic, YilBitis) },
                ["Opel"] = new[] { ("Corsa", YilBaslangic, YilBitis), ("Astra", YilBaslangic, YilBitis), ("Insignia", YilBaslangic, YilBitis) },
                ["Dacia"] = new[] { ("Sandero", YilBaslangic, YilBitis), ("Logan", YilBaslangic, YilBitis), ("Duster", YilBaslangic, YilBitis) },
                ["Mazda"] = new[] { ("3", YilBaslangic, YilBitis), ("6", YilBaslangic, YilBitis), ("2", YilBaslangic, YilBitis) },
                ["Skoda"] = new[] { ("Octavia", YilBaslangic, YilBitis), ("Fabia", YilBaslangic, YilBitis), ("Superb", YilBaslangic, YilBitis) },
                ["Seat"] = new[] { ("Leon", YilBaslangic, YilBitis), ("Ibiza", YilBaslangic, YilBitis), ("Arona", YilBaslangic, YilBitis) },
                ["Togg"] = new[] { ("T10X", 2023, YilBitis), ("T10F", 2025, YilBitis) },
            };
            return (markalar, modeller);
        }

        if (kategori == AracKategorisi.SUV)
        {
            var markalar = new List<Marka>
            {
                new() { Ad = "Toyota", Slug = "toyota-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "BMW", Slug = "bmw-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Mercedes-Benz", Slug = "mercedes-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Volkswagen", Slug = "vw-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Ford", Slug = "ford-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Dacia", Slug = "dacia-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Hyundai", Slug = "hyundai-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Kia", Slug = "kia-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Nissan", Slug = "nissan-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Jeep", Slug = "jeep", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Land Rover", Slug = "land-rover", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Audi", Slug = "audi-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
                new() { Ad = "Peugeot", Slug = "peugeot-suv", Sira = sira++, Kategori = AracKategorisi.SUV },
            };
            var modeller = new Dictionary<string, (string, int, int)[]>
            {
                ["Toyota"] = new[] { ("RAV4", YilBaslangic, YilBitis), ("C-HR", YilBaslangic, YilBitis), ("Land Cruiser", YilBaslangic, YilBitis), ("Highlander", YilBaslangic, YilBitis) },
                ["BMW"] = new[] { ("X1", YilBaslangic, YilBitis), ("X3", YilBaslangic, YilBitis), ("X5", YilBaslangic, YilBitis) },
                ["Mercedes-Benz"] = new[] { ("GLA", YilBaslangic, YilBitis), ("GLC", YilBaslangic, YilBitis), ("GLE", YilBaslangic, YilBitis) },
                ["Volkswagen"] = new[] { ("T-Roc", YilBaslangic, YilBitis), ("Tiguan", YilBaslangic, YilBitis), ("Touareg", YilBaslangic, YilBitis) },
                ["Ford"] = new[] { ("Puma", YilBaslangic, YilBitis), ("Kuga", YilBaslangic, YilBitis), ("Explorer", YilBaslangic, YilBitis) },
                ["Dacia"] = new[] { ("Duster", YilBaslangic, YilBitis), ("Jogger", 2022, YilBitis) },
                ["Hyundai"] = new[] { ("Kona", YilBaslangic, YilBitis), ("Tucson", YilBaslangic, YilBitis), ("Santa Fe", YilBaslangic, YilBitis) },
                ["Kia"] = new[] { ("Stonic", YilBaslangic, YilBitis), ("Sportage", YilBaslangic, YilBitis), ("Sorento", YilBaslangic, YilBitis) },
                ["Nissan"] = new[] { ("Juke", YilBaslangic, YilBitis), ("Qashqai", YilBaslangic, YilBitis), ("X-Trail", YilBaslangic, YilBitis) },
                ["Jeep"] = new[] { ("Renegade", YilBaslangic, YilBitis), ("Compass", YilBaslangic, YilBitis), ("Wrangler", YilBaslangic, YilBitis) },
                ["Land Rover"] = new[] { ("Evoque", YilBaslangic, YilBitis), ("Discovery Sport", YilBaslangic, YilBitis), ("Defender", YilBaslangic, YilBitis) },
                ["Audi"] = new[] { ("Q3", YilBaslangic, YilBitis), ("Q5", YilBaslangic, YilBitis), ("Q7", YilBaslangic, YilBitis) },
                ["Peugeot"] = new[] { ("2008", YilBaslangic, YilBitis), ("3008", YilBaslangic, YilBitis), ("5008", YilBaslangic, YilBitis) },
            };
            return (markalar, modeller);
        }

        if (kategori == AracKategorisi.Pickup)
        {
            var markalar = new List<Marka>
            {
                new() { Ad = "Ford", Slug = "ford-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Toyota", Slug = "toyota-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Fiat", Slug = "fiat-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Mitsubishi", Slug = "mitsubishi-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Isuzu", Slug = "isuzu", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Nissan", Slug = "nissan-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
                new() { Ad = "Mercedes-Benz", Slug = "mercedes-pickup", Sira = sira++, Kategori = AracKategorisi.Pickup },
            };
            var modeller = new Dictionary<string, (string, int, int)[]>
            {
                ["Ford"] = new[] { ("Ranger", YilBaslangic, YilBitis), ("F-150", YilBaslangic, YilBitis) },
                ["Toyota"] = new[] { ("Hilux", YilBaslangic, YilBitis), ("Land Cruiser Pickup", YilBaslangic, YilBitis) },
                ["Fiat"] = new[] { ("Fullback", YilBaslangic, YilBitis), ("Strada", YilBaslangic, YilBitis) },
                ["Mitsubishi"] = new[] { ("L200", YilBaslangic, YilBitis), ("Triton", YilBaslangic, YilBitis) },
                ["Isuzu"] = new[] { ("D-Max", YilBaslangic, YilBitis) },
                ["Nissan"] = new[] { ("Navara", YilBaslangic, YilBitis) },
                ["Mercedes-Benz"] = new[] { ("X-Serisi", YilBaslangic, YilBitis) },
            };
            return (markalar, modeller);
        }

        return (new List<Marka>(), new Dictionary<string, (string, int, int)[]>());
    }

    private static async Task RolleriTohumlaAsync(IServiceProvider servisler, CancellationToken iptal)
    {
        var roleManager = servisler.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var rol in new[] { "Standart", "Premium", "Bayi", "Admin" })
        {
            if (await roleManager.RoleExistsAsync(rol)) continue;
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }
}
