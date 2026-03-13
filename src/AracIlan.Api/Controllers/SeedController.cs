using AracIlan.Altyapi.Veritabani;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Veri tohumu - Marka/model boşsa doldurur
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SeedController(AracIlanVeritabani db, IServiceProvider servisler, ILogger<SeedController> log) : ControllerBase
{
    /// <summary>
    /// Markalar boşsa veri tohumu çalıştırır. İlan ver sayfasında marka/model dolu olması için.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Tohumla(CancellationToken iptal = default)
    {
        if (await db.Markalar.AnyAsync(iptal))
            return Ok(new { mesaj = "Veri zaten mevcut." });

        try
        {
            await VeriTohumu.TohumlaAsync(db, servisler, iptal);
            return Ok(new { mesaj = "Marka, model ve paket verileri eklendi." });
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Veri tohumu hatası");
            return StatusCode(500, new { hata = ex.Message });
        }
    }
}
