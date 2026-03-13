using AracIlan.Uygulama.Depolar;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// AI API - Teknik özellik üretimi (motor seçildiğinde otomatik doldurma)
/// </summary>
[ApiController]
[Route("api/ai")]
public class AIController(IAIServisi aiServisi, IMotorSecenegiDeposu motorDeposu) : ControllerBase
{
    /// <summary>
    /// Motor seçildiğinde teknik özellikleri AI ile üretir
    /// İlan formunda otomatik doldurma için kullanılır
    /// </summary>
    /// <param name="motorId">Motor seçeneği ID</param>
    /// <param name="uretimYili">Üretim yılı</param>
    [HttpGet("teknik-ozellik")]
    public async Task<IActionResult> TeknikOzellikUret([FromQuery] int motorId, [FromQuery] int uretimYili, CancellationToken iptal = default)
    {
        var motor = await motorDeposu.IdIleGetirAsync(motorId, iptal);
        if (motor == null) return NotFound();

        var marka = motor.ModelPaketi.Model.Marka.Ad;
        var model = motor.ModelPaketi.Model.Ad;

        var json = await aiServisi.TeknikOzellikUretAsync(
            marka, model, motor.Ad, motor.MotorHacmi, motor.YakitTipi, motor.Guc, uretimYili, iptal);

        return json != null ? Ok(new { teknikOzellikler = json }) : Ok(new { teknikOzellikler = (string?)null });
    }
}
