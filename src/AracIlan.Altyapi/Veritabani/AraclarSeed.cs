using AracIlan.Alan.Sabitler;
using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Kimlik;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Veritabani;

/// <summary>
/// 100 araç ilanı seed - Gerçek araç fotoğrafları (Unsplash)
/// </summary>
public static class AraclarSeed
{
    private static readonly string[] GorselUrlleri =
    [
        "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?w=800",
        "https://images.unsplash.com/photo-1502877338535-766e1452684a?w=800",
        "https://images.unsplash.com/photo-1549317661-bd32c8ce0db2?w=800",
        "https://images.unsplash.com/photo-1542362567-b07e54358753?w=800",
        "https://images.unsplash.com/photo-1553440569-bcc63803a83d?w=800",
        "https://images.unsplash.com/photo-1580273916550-e323be2ae537?w=800",
        "https://images.unsplash.com/photo-1544636331-e26879cd4d9b?w=800",
        "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?w=800",
        "https://images.unsplash.com/photo-1503376780353-7e6692767b70?w=800",
        "https://images.unsplash.com/photo-1541899481282-d53bfe3c71d3?w=800",
        "https://images.unsplash.com/photo-1552519507-da3b142c6e3d?w=800",
        "https://images.unsplash.com/photo-1605559424843-9e4c228bf1c2?w=800",
        "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?w=800",
        "https://images.unsplash.com/photo-1619767886558-efdc259cde1a?w=800",
        "https://images.unsplash.com/photo-1616422285623-13ff0162193c?w=800",
        "https://images.unsplash.com/photo-1617814076367-b759c7d7e738?w=800",
        "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?w=800",
        "https://images.unsplash.com/photo-1621007947382-bb3c3994e3f7?w=800",
        "https://images.unsplash.com/photo-1616422285623-13ff0162193c?w=800",
        "https://images.unsplash.com/photo-1609521263047-f8f205293f24?w=800",
    ];

    private static readonly string[] Renkler = ["Beyaz", "Siyah", "Gri", "Gümüş", "Kırmızı", "Mavi", "Yeşil", "Bej"];
    private static readonly string[] VitesTipleri = ["Manuel", "Otomatik", "Yarı Otomatik"];

    public static async Task<int> AraclariTohumlaAsync(AracIlanVeritabani db, UserManager<Kullanici> userManager, CancellationToken iptal = default)
    {
        if (await db.Araclar.IgnoreQueryFilters().CountAsync(iptal) >= 100)
            return 0;

        var kullanici = await userManager.FindByEmailAsync("demo@aracilan.com");
        if (kullanici == null)
        {
            kullanici = new Kullanici
            {
                UserName = "demo@aracilan.com",
                Email = "demo@aracilan.com",
                Ad = "Demo",
                Soyad = "Kullanıcı",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(kullanici, "Demo123!");
        }

        var motorIds = await db.MotorSecenekleri
            .Include(m => m.ModelPaketi).ThenInclude(p => p!.Model).ThenInclude(m => m!.Marka)
            .Where(m => m.ModelPaketi != null && m.ModelPaketi.Model != null && m.ModelPaketi.Model.Marka != null)
            .Select(m => m.Id)
            .Take(200)
            .ToListAsync(iptal);

        if (motorIds.Count == 0) return 0;

        var rnd = new Random(42);
        var eklenecek = new List<Arac>();
        var mevcutSayi = await db.Araclar.IgnoreQueryFilters().CountAsync(iptal);
        var hedef = 100 - mevcutSayi;
        if (hedef <= 0) return 0;

        for (var i = 0; i < hedef; i++)
        {
            var motorId = motorIds[rnd.Next(motorIds.Count)];
            var motor = await db.MotorSecenekleri
                .Include(m => m.ModelPaketi!)
                .ThenInclude(p => p!.Model)
                .ThenInclude(m => m!.Marka)
                .FirstOrDefaultAsync(m => m.Id == motorId, iptal);
            if (motor?.ModelPaketi?.Model?.Marka == null) continue;

            var yil = rnd.Next(2018, 2025);
            var km = rnd.Next(5000, 250000);
            var fiyat = (decimal)(rnd.Next(200000, 3500000) / 1000 * 1000);
            var gorselIndex = rnd.Next(GorselUrlleri.Length);

            var arac = new Arac
            {
                Baslik = $"{motor.ModelPaketi.Model.Marka.Ad} {motor.ModelPaketi.Model.Ad} {yil}",
                Aciklama = "Özenle kullanılmış, bakımlı araç. Detaylı bilgi için iletişime geçin.",
                Kategori = (AracKategorisi)(rnd.Next(1, 4)),
                UretimYili = yil,
                Kilometre = km,
                Fiyat = fiyat,
                Renk = Renkler[rnd.Next(Renkler.Length)],
                VitesTipi = VitesTipleri[rnd.Next(VitesTipleri.Length)],
                HasarDurumu = (HasarDurumu)rnd.Next(0, 4),
                IlanDurumu = IlanDurumu.Yayinda,
                KullaniciId = kullanici.Id,
                MotorSecenegiId = motorId,
                IlanBitisTarihi = DateTime.UtcNow.AddDays(30)
            };

            arac.Gorseller.Add(new AracGorseli { DosyaYolu = GorselUrlleri[gorselIndex], Sira = 1 });
            eklenecek.Add(arac);
        }

        db.Araclar.AddRange(eklenecek);
        await db.SaveChangesAsync(iptal);
        return eklenecek.Count;
    }
}
