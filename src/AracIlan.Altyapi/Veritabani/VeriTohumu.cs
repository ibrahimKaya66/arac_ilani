using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AracIlan.Altyapi.Veritabani;

/// <summary>
/// İlk veri tohumu - Roller, marka/model, üyelik paketleri
/// Türkiye pazarına uygun zengin marka-model verisi
/// </summary>
public static class VeriTohumu
{
    public static async Task TohumlaAsync(AracIlanVeritabani db, IServiceProvider servisler, CancellationToken iptal = default)
    {
        await RolleriTohumlaAsync(servisler, iptal);
        if (await db.Markalar.AnyAsync(iptal)) return;

        // Üyelik paketleri (ilk kurulumda)
        if (!await db.UyelikPaketleri.AnyAsync(iptal))
        {
            db.UyelikPaketleri.AddRange(
            new UyelikPaketi { Ad = "Standart", Aciklama = "3 ilan, 8 fotoğraf, 30 gün", Fiyat = 0, IlanHakki = 3, MaksimumFotograf = 8, IlanSuresiGun = 30, Sira = 1 },
            new UyelikPaketi { Ad = "Premium", Aciklama = "10 ilan, 24 fotoğraf, 60 gün", Fiyat = 199, IlanHakki = 10, MaksimumFotograf = 24, IlanSuresiGun = 60, Sira = 2 }
            );
            await db.SaveChangesAsync(iptal);
        }

        // OTOMOBİL MARKALARI
        var markalar = new List<Marka> {
            new() { Ad = "Toyota", Slug = "toyota", Sira = 1, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Honda", Slug = "honda", Sira = 2, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Volkswagen", Slug = "volkswagen", Sira = 3, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Renault", Slug = "renault", Sira = 4, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Fiat", Slug = "fiat", Sira = 5, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Ford", Slug = "ford", Sira = 6, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Hyundai", Slug = "hyundai", Sira = 7, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Kia", Slug = "kia", Sira = 8, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "BMW", Slug = "bmw", Sira = 9, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Mercedes-Benz", Slug = "mercedes-benz", Sira = 10, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Audi", Slug = "audi", Sira = 11, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Nissan", Slug = "nissan", Sira = 12, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Peugeot", Slug = "peugeot", Sira = 13, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Citroën", Slug = "citroen", Sira = 14, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Opel", Slug = "opel", Sira = 15, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Dacia", Slug = "dacia", Sira = 16, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Mazda", Slug = "mazda", Sira = 17, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Skoda", Slug = "skoda", Sira = 18, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Seat", Slug = "seat", Sira = 19, Kategori = AracKategorisi.Otomobil },
            new() { Ad = "Togg", Slug = "togg", Sira = 20, Kategori = AracKategorisi.Otomobil },
            // SUV markaları (Türkiye'de popüler)
            new() { Ad = "Toyota SUV", Slug = "toyota-suv", Sira = 21, Kategori = AracKategorisi.SUV },
            new() { Ad = "BMW SUV", Slug = "bmw-suv", Sira = 22, Kategori = AracKategorisi.SUV },
            new() { Ad = "Mercedes SUV", Slug = "mercedes-suv", Sira = 23, Kategori = AracKategorisi.SUV },
            new() { Ad = "Volkswagen SUV", Slug = "vw-suv", Sira = 24, Kategori = AracKategorisi.SUV },
            new() { Ad = "Ford SUV", Slug = "ford-suv", Sira = 25, Kategori = AracKategorisi.SUV },
            new() { Ad = "Dacia SUV", Slug = "dacia-suv", Sira = 26, Kategori = AracKategorisi.SUV },
            new() { Ad = "Hyundai SUV", Slug = "hyundai-suv", Sira = 27, Kategori = AracKategorisi.SUV },
            new() { Ad = "Kia SUV", Slug = "kia-suv", Sira = 28, Kategori = AracKategorisi.SUV },
            new() { Ad = "Nissan SUV", Slug = "nissan-suv", Sira = 29, Kategori = AracKategorisi.SUV },
            new() { Ad = "Jeep", Slug = "jeep", Sira = 30, Kategori = AracKategorisi.SUV },
            new() { Ad = "Land Rover", Slug = "land-rover", Sira = 31, Kategori = AracKategorisi.SUV },
            // Pickup markaları
            new() { Ad = "Ford Pickup", Slug = "ford-pickup", Sira = 32, Kategori = AracKategorisi.Pickup },
            new() { Ad = "Toyota Pickup", Slug = "toyota-pickup", Sira = 33, Kategori = AracKategorisi.Pickup },
            new() { Ad = "Fiat Pickup", Slug = "fiat-pickup", Sira = 34, Kategori = AracKategorisi.Pickup },
            new() { Ad = "Mitsubishi Pickup", Slug = "mitsubishi-pickup", Sira = 35, Kategori = AracKategorisi.Pickup },
            new() { Ad = "Isuzu", Slug = "isuzu", Sira = 36, Kategori = AracKategorisi.Pickup },
        };
        db.Markalar.AddRange(markalar);
        await db.SaveChangesAsync(iptal);

        // OTOMOBİL MODELLERİ
        var modeller = new List<Model>();
        foreach (var m in markalar)
        {
            var modellerMarka = m.Ad switch
            {
                "Toyota" => new[] { ("Corolla", 2018, 2025), ("Yaris", 2019, 2025), ("Camry", 2018, 2025), ("Auris", 2018, 2022) },
                "Honda" => new[] { ("Civic", 2017, 2025), ("Jazz", 2015, 2022), ("Accord", 2018, 2025) },
                "Volkswagen" => new[] { ("Golf", 2017, 2025), ("Polo", 2017, 2025), ("Passat", 2018, 2025), ("Jetta", 2018, 2024) },
                "Renault" => new[] { ("Clio", 2019, 2025), ("Megane", 2018, 2025), ("Fluence", 2016, 2022), ("Talisman", 2018, 2024) },
                "Fiat" => new[] { ("Egea", 2016, 2025), ("Linea", 2012, 2018), ("Tipo", 2018, 2025) },
                "Ford" => new[] { ("Focus", 2018, 2025), ("Fiesta", 2017, 2023), ("Mondeo", 2018, 2022) },
                "Hyundai" => new[] { ("i20", 2018, 2025), ("i30", 2017, 2025), ("Elantra", 2018, 2025), ("Accent", 2018, 2024) },
                "Kia" => new[] { ("Ceed", 2018, 2025), ("Rio", 2017, 2025), ("Cerato", 2018, 2025), ("Optima", 2018, 2022) },
                "BMW" => new[] { ("3 Serisi", 2019, 2025), ("5 Serisi", 2018, 2025), ("1 Serisi", 2019, 2025) },
                "Mercedes-Benz" => new[] { ("C Serisi", 2018, 2025), ("E Serisi", 2018, 2025), ("A Serisi", 2018, 2025) },
                "Audi" => new[] { ("A3", 2018, 2025), ("A4", 2018, 2025), ("A6", 2018, 2025) },
                "Nissan" => new[] { ("Qashqai", 2018, 2025), ("Juke", 2019, 2025), ("Leaf", 2018, 2025), ("Micra", 2017, 2025) },
                "Peugeot" => new[] { ("208", 2019, 2025), ("308", 2018, 2025), ("508", 2018, 2025), ("301", 2017, 2025) },
                "Citroën" => new[] { ("C3", 2018, 2025), ("C4", 2018, 2025), ("C4 Cactus", 2018, 2022) },
                "Opel" => new[] { ("Corsa", 2019, 2025), ("Astra", 2018, 2025), ("Insignia", 2018, 2025) },
                "Dacia" => new[] { ("Sandero", 2018, 2025), ("Logan", 2017, 2025), ("Duster", 2018, 2025) },
                "Mazda" => new[] { ("3", 2019, 2025), ("6", 2018, 2025), ("2", 2018, 2025) },
                "Skoda" => new[] { ("Octavia", 2018, 2025), ("Fabia", 2018, 2025), ("Superb", 2018, 2025) },
                "Seat" => new[] { ("Leon", 2018, 2025), ("Ibiza", 2018, 2025), ("Arona", 2018, 2025) },
                "Togg" => new[] { ("T10X", 2023, 2025), ("T10F", 2025, 2025) },
                "Toyota SUV" => new[] { ("RAV4", 2019, 2025), ("C-HR", 2018, 2025), ("Land Cruiser", 2018, 2025), ("Highlander", 2020, 2025) },
                "BMW SUV" => new[] { ("X1", 2019, 2025), ("X3", 2018, 2025), ("X5", 2018, 2025) },
                "Mercedes SUV" => new[] { ("GLA", 2018, 2025), ("GLC", 2018, 2025), ("GLE", 2018, 2025) },
                "Volkswagen SUV" => new[] { ("T-Roc", 2019, 2025), ("Tiguan", 2018, 2025), ("Touareg", 2018, 2025) },
                "Ford SUV" => new[] { ("Puma", 2019, 2025), ("Kuga", 2018, 2025), ("Explorer", 2018, 2025) },
                "Dacia SUV" => new[] { ("Duster", 2018, 2025), ("Jogger", 2022, 2025) },
                "Hyundai SUV" => new[] { ("Kona", 2018, 2025), ("Tucson", 2018, 2025), ("Santa Fe", 2018, 2025) },
                "Kia SUV" => new[] { ("Stonic", 2018, 2025), ("Sportage", 2018, 2025), ("Sorento", 2018, 2025) },
                "Nissan SUV" => new[] { ("Juke", 2019, 2025), ("Qashqai", 2018, 2025), ("X-Trail", 2018, 2025) },
                "Jeep" => new[] { ("Renegade", 2018, 2025), ("Compass", 2018, 2025), ("Wrangler", 2018, 2025) },
                "Land Rover" => new[] { ("Evoque", 2018, 2025), ("Discovery Sport", 2018, 2025), ("Defender", 2020, 2025) },
                "Ford Pickup" => new[] { ("Ranger", 2018, 2025), ("F-150", 2018, 2025) },
                "Toyota Pickup" => new[] { ("Hilux", 2018, 2025), ("Land Cruiser Pickup", 2018, 2025) },
                "Fiat Pickup" => new[] { ("Fullback", 2016, 2022), ("Strada", 2018, 2025) },
                "Mitsubishi Pickup" => new[] { ("L200", 2018, 2025), ("Triton", 2018, 2025) },
                "Isuzu" => new[] { ("D-Max", 2018, 2025) },
                _ => new[] { ("Model", 2020, 2025) }
            };
            foreach (var (ad, baslangic, bitis) in modellerMarka)
            {
                modeller.Add(new Model { Ad = ad, Slug = ad.ToLowerInvariant().Replace(" ", "-"), MarkaId = m.Id, UretimBaslangicYili = baslangic, UretimBitisYili = bitis });
            }
        }
        db.Modeller.AddRange(modeller);
        await db.SaveChangesAsync(iptal);

        // Her model için basit paket ve motor
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
