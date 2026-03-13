using AracIlan.Sozlesmeler.Istekler;
using AracIlan.Sozlesmeler.Yanitlar;
using AracIlan.Uygulama.Depolar;

namespace AracIlan.Uygulama.Servisler;

/// <summary>
/// İlan servisi arayüzü
/// </summary>
public interface IIlanServisi
{
    Task<IlanListeSayfaliYaniti> FiltreliGetirAsync(AracFiltre filtre, CancellationToken iptal = default);
    Task<AracDetayYaniti?> DetayGetirAsync(int id, CancellationToken iptal = default);
    Task<(bool Yeterli, int KalanHak, int MaksFotograf)> IlanHakkiKontrolAsync(string kullaniciId, int fotografSayisi, CancellationToken iptal = default);
    Task<int> IlanSuresiGunGetirAsync(string kullaniciId, CancellationToken iptal = default);
    Task<int> OlusturAsync(IlanOlusturmaIstegi istek, string kullaniciId, CancellationToken iptal = default);
    Task<bool> YayinlaAsync(int ilanId, string kullaniciId, CancellationToken iptal = default);
    Task<bool> SatildiOlarakIsaretleAsync(int ilanId, string kullaniciId, CancellationToken iptal = default);
    Task<IlanListeSayfaliYaniti> KullaniciIlanlariniGetirAsync(string kullaniciId, int sayfa, int sayfaBoyutu, CancellationToken iptal = default);
    Task<bool> GuncelleAsync(int ilanId, IlanGuncellemeIstegi istek, string kullaniciId, CancellationToken iptal = default);
}
