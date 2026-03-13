using AracIlan.Alan.Varliklar;

namespace AracIlan.Uygulama.Depolar;

/// <summary>
/// Kullanıcı abonelik veri erişimi - İlan hakkı kontrolü için
/// </summary>
public interface IKullaniciAbonelikDeposu
{
    /// <summary>Kullanıcının aktif abonelik bilgisini getirir (en son bitiş tarihi)</summary>
    Task<(int IlanHakki, int MaksimumFotograf, int IlanSuresiGun)> AktifAbonelikGetirAsync(string kullaniciId, CancellationToken iptal = default);

    /// <summary>Kullanıcının kullandığı ilan sayısı (yayında + taslak)</summary>
    Task<int> KullanilanIlanSayisiAsync(string kullaniciId, CancellationToken iptal = default);
}
