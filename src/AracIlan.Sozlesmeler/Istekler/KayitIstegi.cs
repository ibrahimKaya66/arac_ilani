using System.ComponentModel.DataAnnotations;

namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// Kullanıcı kayıt isteği
/// </summary>
public record KayitIstegi(
    [EmailAddress, Required, MinLength(3), MaxLength(256)]
    string Email,

    [Required, MinLength(6, ErrorMessage = "Şifre en az 6 karakter, büyük harf, küçük harf, rakam ve noktalama işareti içermelidir")]
    string Sifre,

    [Required, MinLength(2), MaxLength(50)]
    string Ad,

    [Required, MinLength(2), MaxLength(50)]
    string Soyad,

    [Phone, MaxLength(20)]
    string? Telefon = null
);
