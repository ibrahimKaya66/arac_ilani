using Microsoft.AspNetCore.Identity;

namespace AracIlan.Altyapi.Kimlik;

/// <summary>
/// Identity kullanıcı varlığı - AspNetUsers tablosu
/// Telefon, ad-soyad gibi ek alanlar burada
/// </summary>
public class Kullanici : IdentityUser
{
    /// <summary>Ad</summary>
    public string Ad { get; set; } = string.Empty;

    /// <summary>Soyad</summary>
    public string Soyad { get; set; } = string.Empty;

    /// <summary>Telefon</summary>
    public string? Telefon { get; set; }

    /// <summary>Kayıt tarihi</summary>
    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;
}
