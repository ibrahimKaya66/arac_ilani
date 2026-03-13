using System.Text.Json.Serialization;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Giriş isteği - E-posta veya Gmail kullanıcı adı (örn: ibrahim.kaya5466)
/// </summary>
public record GirisIstegi(
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("sifre")] string? Sifre
);
