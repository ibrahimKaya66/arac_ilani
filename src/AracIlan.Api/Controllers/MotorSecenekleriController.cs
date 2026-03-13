using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Motor seçeneği API - Paket seçildiğinde motorlar (AI teknik özellik dolduracak)
/// </summary>
[ApiController]
[Route("api/paketler/{paketId:int}/motor-secenekleri")]
public class MotorSecenekleriController : ControllerBase
{
    private readonly MotorSecenegiServisi _motorServisi;

    public MotorSecenekleriController(MotorSecenegiServisi motorServisi)
    {
        _motorServisi = motorServisi;
    }

    /// <summary>
    /// Pakete ait motor seçeneklerini getirir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PaketIdIleGetir(int paketId, CancellationToken iptal = default)
    {
        var motorlar = await _motorServisi.PaketIdIleGetirAsync(paketId, iptal);
        return Ok(motorlar);
    }
}
