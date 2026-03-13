namespace AracIlan.Sozlesmeler.Istekler;

/// <summary>
/// İlan güncelleme isteği - Sadece değiştirilebilir alanlar
/// </summary>
public record IlanGuncellemeIstegi(
    int? Kilometre,
    decimal? Fiyat,
    string? Renk,
    string? VitesTipi,
    string? Aciklama,
    int? HasarDurumu,
    List<string>? GorselYollari = null,
    string? ExpertizGorselYolu = null
);
