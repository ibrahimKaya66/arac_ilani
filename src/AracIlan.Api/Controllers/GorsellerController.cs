using System.Security.Claims;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Dosya yükleme - Araç fotoğrafları, expertiz
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GorsellerController(IDosyaServisi dosyaServisi) : ControllerBase
{
    /// <summary>
    /// Araç fotoğrafı yükle - İlan oluştururken kullan
    /// </summary>
    [HttpPost("arac")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> AracGorseliYukle(IFormFile dosya, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        if (dosya is not { Length: > 0 })
            return BadRequest(new { hata = "Dosya seçilmedi" });

        try
        {
            await using var stream = dosya.OpenReadStream();
            var yol = await dosyaServisi.AraçGorseliKaydetAsync(stream, dosya.FileName, kullaniciId, iptal);
            return Ok(new { yol });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { hata = ex.Message });
        }
    }

    /// <summary>
    /// Expertiz görseli yükle
    /// </summary>
    [HttpPost("expertiz")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> ExpertizGorseliYukle(IFormFile dosya, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        if (dosya is not { Length: > 0 })
            return BadRequest(new { hata = "Dosya seçilmedi" });

        try
        {
            await using var stream = dosya.OpenReadStream();
            var yol = await dosyaServisi.ExpertizGorseliKaydetAsync(stream, dosya.FileName, kullaniciId, iptal);
            return Ok(new { yol });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { hata = ex.Message });
        }
    }
}
