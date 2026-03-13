using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AracIlan.Sozlesmeler.Istekler;
using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Altyapi.Kimlik;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Kimlik doğrulama - Kayıt, Giriş
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class KimlikController(
    UserManager<Kullanici> kullaniciYoneticisi,
    IConfiguration yapilandirma) : ControllerBase
{
    [HttpPost("kayit")]
    public async Task<IActionResult> Kayit([FromBody] KayitIstegi istek, CancellationToken iptal = default)
    {
        var kullanici = new Kullanici
        {
            UserName = istek.Email,
            Email = istek.Email,
            Ad = istek.Ad,
            Soyad = istek.Soyad,
            Telefon = istek.Telefon,
            EmailConfirmed = true
        };

        var sonuc = await kullaniciYoneticisi.CreateAsync(kullanici, istek.Sifre);
        if (!sonuc.Succeeded)
            return BadRequest(sonuc.Errors.Select(e => e.Description));

        await kullaniciYoneticisi.AddToRoleAsync(kullanici, "Standart");

        var token = await TokenOlusturAsync(kullanici, iptal);
        return Ok(token);
    }

    [HttpPost("giris")]
    public async Task<IActionResult> Giris([FromBody] GirisIstegi istek, CancellationToken iptal = default)
    {
        var kullanici = await kullaniciYoneticisi.FindByEmailAsync(istek.Email);
        if (kullanici == null || !await kullaniciYoneticisi.CheckPasswordAsync(kullanici, istek.Sifre))
            return Unauthorized("E-posta veya şifre hatalı");

        var token = await TokenOlusturAsync(kullanici, iptal);
        return Ok(token);
    }

    private async Task<GirisYaniti> TokenOlusturAsync(Kullanici kullanici, CancellationToken iptal)
    {
        var roller = await kullaniciYoneticisi.GetRolesAsync(kullanici);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, kullanici.Id),
            new(ClaimTypes.Email, kullanici.Email ?? ""),
            new(ClaimTypes.Name, $"{kullanici.Ad} {kullanici.Soyad}")
        };

        foreach (var rol in roller)
            claims.Add(new Claim(ClaimTypes.Role, rol));

        var anahtar = yapilandirma["Jwt:Anahtar"] ?? "AracIlan-Gizli-Anahtar-En-Az-32-Karakter-Olmali-2024";
        var simetrikAnahtar = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(anahtar));
        var kimlikBilgileri = new SigningCredentials(simetrikAnahtar, SecurityAlgorithms.HmacSha256);

        var sure = int.TryParse(yapilandirma["Jwt:SureDakika"], out var dk) ? dk : 60;
        var token = new JwtSecurityToken(
            issuer: yapilandirma["Jwt:Yayinci"],
            audience: yapilandirma["Jwt:HedefKitle"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(sure),
            signingCredentials: kimlikBilgileri
        );

        var tokenDizisi = new JwtSecurityTokenHandler().WriteToken(token);
        return new GirisYaniti(
            tokenDizisi,
            DateTime.UtcNow.AddMinutes(sure),
            kullanici.Id,
            kullanici.Email ?? "",
            kullanici.Ad,
            kullanici.Soyad,
            [.. roller]
        );
    }
}
