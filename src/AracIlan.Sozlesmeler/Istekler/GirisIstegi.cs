using System.ComponentModel.DataAnnotations;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Giriş isteği
/// </summary>
public record GirisIstegi(
    [EmailAddress, Required]
    string Email,

    [Required]
    string Sifre
);
