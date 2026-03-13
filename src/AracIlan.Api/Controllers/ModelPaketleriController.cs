using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Model paketi API - Yıl girildiğinde o yıla ait paketler
/// </summary>
[ApiController]
[Route("api/modeller/{modelId:int}/paketler")]
public class ModelPaketleriController : ControllerBase
{
    private readonly ModelPaketiServisi _paketServisi;

    public ModelPaketleriController(ModelPaketiServisi paketServisi)
    {
        _paketServisi = paketServisi;
    }

    /// <summary>
    /// Model ve yıla göre paketleri getirir
    /// </summary>
    /// <param name="modelId">Model ID</param>
    /// <param name="yil">Üretim yılı (örn: 2022)</param>
    [HttpGet]
    public async Task<IActionResult> ModelVeYilIleGetir(int modelId, [FromQuery] int yil, CancellationToken iptal = default)
    {
        var paketler = await _paketServisi.ModelVeYilIleGetirAsync(modelId, yil, iptal);
        return Ok(paketler);
    }
}
