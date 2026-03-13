using AracIlan.Alan.Sabitler;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Marka API - Kategoriye göre markalar (Otomobil, SUV, Pickup)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MarkalarController : ControllerBase
{
    private readonly MarkaServisi _markaServisi;

    public MarkalarController(MarkaServisi markaServisi)
    {
        _markaServisi = markaServisi;
    }

    /// <summary>
    /// Kategoriye göre markaları getirir
    /// </summary>
    /// <param name="kategori">1=Otomobil, 2=SUV, 3=Pickup</param>
    [HttpGet]
    public async Task<IActionResult> KategoriyeGoreGetir([FromQuery] AracKategorisi kategori = AracKategorisi.Otomobil, CancellationToken iptal = default)
    {
        var markalar = await _markaServisi.KategoriyeGoreGetirAsync(kategori, iptal);
        return Ok(markalar);
    }
}
