using System.ComponentModel.DataAnnotations;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Giriş isteği
/// </summary>
public record GirisIstegi(
    [property: EmailAddress, Required]
    string Email,

    [property: Required]
    string Sifre
);
