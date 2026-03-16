using System.Security.Claims;
using AracIlan.Alan.Sabitler;
using AracIlan.Sozlesmeler.Istekler;
using AracIlan.Uygulama.Depolar;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AracIlan.Api.Controllers;

/// <summary>
/// İlan API - Listeleme, detay, oluşturma
/// </summary>
[ApiController]
[Route("api/ilanlar")]
public class IlanlarController(IIlanServisi ilanServisi) : ControllerBase
{

    /// <summary>
    /// Filtreli ilan listesi
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> FiltreliGetir(
        [FromQuery] AracKategorisi? kategori,
        [FromQuery] int? markaId,
        [FromQuery] int? modelId,
        [FromQuery] int? minYil,
        [FromQuery] int? maxYil,
        [FromQuery] decimal? minFiyat,
        [FromQuery] decimal? maxFiyat,
        [FromQuery] int? minKm,
        [FromQuery] int? maxKm,
        [FromQuery] int sayfa = 1,
        [FromQuery] int sayfaBoyutu = 20,
        [FromQuery] string? siralama = null,
        CancellationToken iptal = default)
    {
        var filtre = new AracFiltre
        {
            Kategori = kategori,
            MarkaId = markaId,
            ModelId = modelId,
            MinYil = minYil,
            MaxYil = maxYil,
            MinFiyat = minFiyat,
            MaxFiyat = maxFiyat,
            MinKm = minKm,
            MaxKm = maxKm,
            Sayfa = sayfa,
            SayfaBoyutu = Math.Clamp(sayfaBoyutu, 1, 50),
            Siralama = siralama
        };

        var sonuc = await ilanServisi.FiltreliGetirAsync(filtre, iptal);
        return Ok(sonuc);
    }

    /// <summary>
    /// İlan detayı
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> DetayGetir(int id, CancellationToken iptal = default)
    {
        var detay = await ilanServisi.DetayGetirAsync(id, iptal);
        if (detay == null) return NotFound();
        return Ok(detay);
    }

    /// <summary>
    /// Yeni ilan oluştur - Giriş gerekli
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Olustur([FromBody] IlanOlusturmaIstegi istek, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Kullanıcı bilgisi alınamadı");

        try
        {
            var id = await ilanServisi.OlusturAsync(istek, kullaniciId, iptal);
            return CreatedAtAction(nameof(DetayGetir), new { id }, new { id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { hata = ex.Message });
        }
    }

    /// <summary>
    /// Taslak ilanı yayınla
    /// </summary>
    [HttpPost("{id:int}/yayinla")]
    [Authorize]
    public async Task<IActionResult> Yayinla(int id, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var basarili = await ilanServisi.YayinlaAsync(id, kullaniciId, iptal);
        return basarili ? Ok() : NotFound();
    }

    /// <summary>
    /// İlanı satıldı olarak işaretle (sadece yayındaki ilanlar)
    /// </summary>
    [HttpPost("{id:int}/satildi")]
    [Authorize]
    public async Task<IActionResult> Satildi(int id, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var basarili = await ilanServisi.SatildiOlarakIsaretleAsync(id, kullaniciId, iptal);
        return basarili ? Ok() : NotFound();
    }

    /// <summary>
    /// Kullanıcının kendi ilanları (taslak + yayında) - Filtre ile
    /// </summary>
    [HttpGet("benim")]
    [Authorize]
    public async Task<IActionResult> BenimIlanlarim(
        [FromQuery] AracKategorisi? kategori,
        [FromQuery] int? markaId,
        [FromQuery] int? modelId,
        [FromQuery] int? minYil,
        [FromQuery] int? maxYil,
        [FromQuery] decimal? minFiyat,
        [FromQuery] decimal? maxFiyat,
        [FromQuery] int? minKm,
        [FromQuery] int? maxKm,
        [FromQuery] int sayfa = 1,
        [FromQuery] int sayfaBoyutu = 20,
        [FromQuery] string? siralama = null,
        CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var filtre = new AracFiltre
        {
            Kategori = kategori,
            MarkaId = markaId,
            ModelId = modelId,
            MinYil = minYil,
            MaxYil = maxYil,
            MinFiyat = minFiyat,
            MaxFiyat = maxFiyat,
            MinKm = minKm,
            MaxKm = maxKm,
            Sayfa = sayfa,
            SayfaBoyutu = Math.Clamp(sayfaBoyutu, 1, 50),
            Siralama = siralama
        };

        var sonuc = await ilanServisi.KullaniciIlanlariniFiltreliGetirAsync(kullaniciId, filtre, iptal);
        return Ok(sonuc);
    }

    /// <summary>
    /// İlan sil - Sadece sahibi silebilir
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Sil(int id, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var basarili = await ilanServisi.SilAsync(id, kullaniciId, iptal);
        return basarili ? Ok() : NotFound();
    }

    /// <summary>
    /// İlan güncelle - Sadece sahibi güncelleyebilir (Taslak veya Yayında)
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Guncelle(int id, [FromBody] IlanGuncellemeIstegi istek, CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var basarili = await ilanServisi.GuncelleAsync(id, istek, kullaniciId, iptal);
        return basarili ? Ok() : NotFound();
    }

    /// <summary>
    /// Kullanıcının ilan hakkı bilgisi
    /// </summary>
    [HttpGet("hakkim")]
    [Authorize]
    public async Task<IActionResult> IlanHakkim(CancellationToken iptal = default)
    {
        var kullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(kullaniciId)) return Unauthorized();

        var (yeterli, kalan, maksFotograf) = await ilanServisi.IlanHakkiKontrolAsync(kullaniciId, 0, iptal);
        var ilanSuresiGun = await ilanServisi.IlanSuresiGunGetirAsync(kullaniciId, iptal);
        return Ok(new { yeterli, kalan, maksFotograf, ilanSuresiGun });
    }
}
