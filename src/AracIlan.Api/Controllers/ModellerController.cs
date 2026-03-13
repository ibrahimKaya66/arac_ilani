using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Model API - Marka seçildiğinde modeller
/// </summary>
[ApiController]
[Route("api/markalar/{markaId:int}/modeller")]
public class ModellerController : ControllerBase
{
    private readonly ModelServisi _modelServisi;

    public ModellerController(ModelServisi modelServisi)
    {
        _modelServisi = modelServisi;
    }

    /// <summary>
    /// Markaya ait modelleri getirir (yıl aralığı ile)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> MarkaIdIleGetir(int markaId, CancellationToken iptal = default)
    {
        var modeller = await _modelServisi.MarkaIdIleGetirAsync(markaId, iptal);
        return Ok(modeller);
    }
}
