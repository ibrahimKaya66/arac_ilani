namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Giriş/Kayıt yanıtı - JWT token ve kullanıcı bilgisi
/// </summary>
public record GirisYaniti(
    string Token,
    DateTime GecerlilikTarihi,
    string KullaniciId,
    string Email,
    string Ad,
    string Soyad,
    IReadOnlyList<string> Roller
);
