using AracIlan.Alan.Sabitler;
using AracIlan.Altyapi.Veritabani;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Veri tohumu - Eksik marka/model verilerini ekler (Otomobil, SUV, Pickup)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SeedController(AracIlanVeritabani db, IServiceProvider servisler, ILogger<SeedController> log) : ControllerBase
{
    /// <summary>
    /// Tüm kategorilerdeki eksik marka/model verilerini ekler. Mevcut veri olsa bile eksikleri tamamlar.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Tohumla(CancellationToken iptal = default)
    {
        try
        {
            foreach (var kategori in new[] { AracKategorisi.Otomobil, AracKategorisi.SUV, AracKategorisi.Pickup })
            {
                await VeriTohumu.TohumlaKategoriEksikseAsync(db, servisler, kategori, iptal);
            }
            return Ok(new { mesaj = "Marka, model ve paket verileri güncellendi." });
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Veri tohumu hatası");
            return StatusCode(500, new { hata = ex.Message });
        }
    }
}
