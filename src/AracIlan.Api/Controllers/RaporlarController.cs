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
}
