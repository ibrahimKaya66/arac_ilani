using System.ComponentModel.DataAnnotations;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Kullanıcı kayıt isteği
/// </summary>
public record KayitIstegi(
    [property: EmailAddress, Required, MinLength(3), MaxLength(256)]
    string Email,

    [property: Required, MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
    string Sifre,

    [property: Required, MinLength(2), MaxLength(50)]
    string Ad,

    [property: Required, MinLength(2), MaxLength(50)]
    string Soyad,

    [property: Phone, MaxLength(20)]
    string? Telefon = null
);
