using AracIlan.Alan.Sabitler;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Marka API - Kategoriye göre markalar (Otomobil, SUV, Pickup)
/// Markalar boşsa otomatik seed çalıştırır
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MarkalarController(
    MarkaServisi markaServisi,
    AracIlanVeritabani db,
    IServiceProvider servisler,
    ILogger<MarkalarController> log) : ControllerBase
{
    /// <summary>
    /// Kategoriye göre markaları getirir. Veri yoksa otomatik yükler.
    /// </summary>
    /// <param name="kategori">1=Otomobil, 2=SUV, 3=Pickup</param>
    [HttpGet]
    public async Task<IActionResult> KategoriyeGoreGetir([FromQuery] AracKategorisi kategori = AracKategorisi.Otomobil, CancellationToken iptal = default)
    {
        var markalar = await markaServisi.KategoriyeGoreGetirAsync(kategori, iptal);

        if (markalar.Count == 0)
        {
            try
            {
                await VeriTohumu.TohumlaKategoriEksikseAsync(db, servisler, kategori, iptal);
                markalar = await markaServisi.KategoriyeGoreGetirAsync(kategori, iptal);
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Marka seed otomatik çalıştırılamadı");
            }
        }

        return Ok(markalar);
    }
}
