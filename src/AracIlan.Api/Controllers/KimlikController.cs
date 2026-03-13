using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AracIlan.Sozlesmeler.Istekler;
using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Altyapi.Kimlik;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AracIlan.Api.Controllers;

/// <summary>
/// Kimlik doğrulama - Kayıt, Giriş, Google OAuth
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class KimlikController(
    UserManager<Kullanici> kullaniciYoneticisi,
    RoleManager<IdentityRole> rolYoneticisi,
    IConfiguration yapilandirma,
    ILogger<KimlikController> log) : ControllerBase
{
    [HttpPost("kayit")]
    public async Task<IActionResult> Kayit([FromBody] KayitIstegi? istek, CancellationToken iptal = default)
    {
        if (istek == null)
            return BadRequest(new { hata = "Geçersiz istek. E-posta, şifre, ad ve soyad gerekli." });

        try
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
            {
                var mesaj = string.Join(" ", sonuc.Errors.Select(e => e.Description));
                return BadRequest(new { hata = mesaj });
            }

            const string standartRol = "Standart";
            if (!await rolYoneticisi.RoleExistsAsync(standartRol))
            {
                await rolYoneticisi.CreateAsync(new IdentityRole(standartRol));
            }

            var rolSonuc = await kullaniciYoneticisi.AddToRoleAsync(kullanici, standartRol);
            if (!rolSonuc.Succeeded)
            {
                var mesaj = string.Join(" ", rolSonuc.Errors.Select(e => e.Description));
                return BadRequest(new { hata = mesaj });
            }

            var token = await TokenOlusturAsync(kullanici, iptal);
            return Ok(token);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Kayıt hatası");
            return StatusCode(500, new { hata = ex.Message });
        }
    }

    /// <summary>
    /// Google ile giriş - Tarayıcıyı Google OAuth'a yönlendirir
    /// </summary>
    [HttpGet("google")]
    public IActionResult Google([FromQuery] string? returnUrl = null)
    {
        var spaUrl = yapilandirma["Spa:BaseUrl"] ?? "http://localhost:3000";
        var clientId = yapilandirma["Google:ClientId"];
        if (string.IsNullOrWhiteSpace(clientId))
        {
            var callback = returnUrl ?? $"{spaUrl}/auth/callback";
            return Redirect($"{callback}?hata=" + Uri.EscapeDataString("Google OAuth yapılandırılmamış. appsettings.Development.json içine Google:ClientId ve Google:ClientSecret ekleyin."));
        }

        var redirectUri = $"{Request.Scheme}://{Request.Host}/api/kimlik/google/token?returnUrl={Uri.EscapeDataString(returnUrl ?? spaUrl)}";
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, "Google");
    }

    /// <summary>
    /// Google OAuth callback sonrası JWT üretir ve SPA'ya yönlendirir
    /// </summary>
    [HttpGet("google/token")]
    public async Task<IActionResult> GoogleToken([FromQuery] string? returnUrl, CancellationToken iptal = default)
    {
        var result = await HttpContext.AuthenticateAsync("ExternalCookie");
        if (!result.Succeeded)
        {
            await HttpContext.SignOutAsync("ExternalCookie");
            return Unauthorized("Google ile giriş başarısız.");
        }

        var principal = result.Principal;
        var email = principal?.FindFirst(ClaimTypes.Email)?.Value
            ?? principal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
            ?? principal?.FindFirst("email")?.Value;
        var adSoyad = principal?.FindFirst(ClaimTypes.Name)?.Value ?? "";

        await HttpContext.SignOutAsync("ExternalCookie");

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("E-posta alınamadı.");
        }

        var parcalar = adSoyad.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var ad = parcalar.Length > 0 ? parcalar[0] : "Kullanıcı";
        var soyad = parcalar.Length > 1 ? parcalar[1] : "";

        var kullanici = await kullaniciYoneticisi.FindByEmailAsync(email);
        if (kullanici == null)
        {
            kullanici = new Kullanici
            {
                UserName = email,
                Email = email,
                Ad = ad,
                Soyad = soyad,
                EmailConfirmed = true
            };
            var createSonuc = await kullaniciYoneticisi.CreateAsync(kullanici);
            if (!createSonuc.Succeeded)
            {
                var mesaj = string.Join(" ", createSonuc.Errors.Select(e => e.Description));
                return BadRequest(new { hata = mesaj });
            }

            const string standartRol = "Standart";
            if (!await rolYoneticisi.RoleExistsAsync(standartRol))
                await rolYoneticisi.CreateAsync(new IdentityRole(standartRol));
            await kullaniciYoneticisi.AddToRoleAsync(kullanici, standartRol);
        }

        var girisYaniti = await TokenOlusturAsync(kullanici, iptal);
        var spaUrl = returnUrl ?? yapilandirma["Spa:BaseUrl"] ?? "http://localhost:3000";
        var callbackUrl = spaUrl.EndsWith("/auth/callback") ? spaUrl : $"{spaUrl.TrimEnd('/')}/auth/callback";
        var sep = callbackUrl.Contains('?') ? "&" : "?";
        return Redirect($"{callbackUrl}{sep}token={Uri.EscapeDataString(girisYaniti.Token)}&kullaniciId={girisYaniti.KullaniciId}&ad={Uri.EscapeDataString(girisYaniti.Ad)}&soyad={Uri.EscapeDataString(girisYaniti.Soyad)}&email={Uri.EscapeDataString(girisYaniti.Email)}&roller={Uri.EscapeDataString(string.Join(",", girisYaniti.Roller))}");
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
