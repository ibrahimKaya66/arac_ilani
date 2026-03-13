using AracIlan.Uygulama.Depolar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Admin raporları - En çok ilan verilen marka/model vb.
/// </summary>
[ApiController]
[Route("api/raporlar")]
[Authorize(Roles = "Admin")]
public class RaporlarController(IRaporDeposu raporDeposu) : ControllerBase
{
    /// <summary>
    /// En çok ilan verilen markalar
    /// </summary>
    [HttpGet("en-cok-ilanli-markalar")]
    public async Task<IActionResult> EnCokIlanliMarkalar([FromQuery] int adet = 10, CancellationToken iptal = default)
    {
        var sonuc = await raporDeposu.EnCokIlanliMarkalarAsync(Math.Clamp(adet, 1, 50), iptal);
        return Ok(sonuc);
    }

    /// <summary>
    /// En çok ilan verilen modeller
    /// </summary>
    [HttpGet("en-cok-ilanli-modeller")]
    public async Task<IActionResult> EnCokIlanliModeller([FromQuery] int adet = 10, CancellationToken iptal = default)
    {
        var sonuc = await raporDeposu.EnCokIlanliModellerAsync(Math.Clamp(adet, 1, 50), iptal);
        return Ok(sonuc);
    }

    /// <summary>
    /// Kategori bazlı ilan sayıları (Otomobil, SUV, Pickup)
    /// </summary>
    [HttpGet("kategori-dagilimi")]
    public async Task<IActionResult> KategoriDagilimi(CancellationToken iptal = default)
    {
        var sonuc = await raporDeposu.KategoriBazliIlanSayilariAsync(iptal);
        return Ok(sonuc);
    }

    /// <summary>
    /// Son X günde eklenen ilan sayısı
    /// </summary>
    [HttpGet("son-gunler")]
    public async Task<IActionResult> SonGunlerdeIlan([FromQuery] int gun = 30, CancellationToken iptal = default)
    {
        var sayi = await raporDeposu.SonGunlerdeIlanSayisiAsync(Math.Clamp(gun, 1, 365), iptal);
        return Ok(new { gun, ilanSayisi = sayi });
    }

    /// <summary>
    /// Tarih aralığında satış raporu (toplam satış, ciro, aktif ilan)
    /// </summary>
    [HttpGet("tarih-araligi")]
    public async Task<IActionResult> TarihAraligiSatis([FromQuery] DateTime? baslangic, [FromQuery] DateTime? bitis, CancellationToken iptal = default)
    {
        var bas = baslangic ?? DateTime.UtcNow.AddMonths(-1);
        var son = bitis ?? DateTime.UtcNow;
        var rapor = await raporDeposu.TarihAraligiSatisRaporuAsync(bas, son, iptal);
        return Ok(rapor);
    }

    /// <summary>
    /// Üretim yılına göre satış dağılımı
    /// </summary>
    [HttpGet("uretim-yili-satis")]
    public async Task<IActionResult> UretimYiliSatis([FromQuery] DateTime? baslangic, [FromQuery] DateTime? bitis, CancellationToken iptal = default)
    {
        var bas = baslangic ?? DateTime.UtcNow.AddMonths(-1);
        var son = bitis ?? DateTime.UtcNow;
        var rapor = await raporDeposu.UretimYilinaGoreSatisAsync(bas, son, iptal);
        return Ok(rapor);
    }

    /// <summary>
    /// En hızlı satılan ilanlar (gün bazında)
    /// </summary>
    [HttpGet("en-hizli-satilanlar")]
    public async Task<IActionResult> EnHizliSatilanlar([FromQuery] int adet = 10, CancellationToken iptal = default)
    {
        var rapor = await raporDeposu.EnHizliSatilanlarAsync(Math.Clamp(adet, 1, 50), iptal);
        return Ok(rapor);
    }
}
