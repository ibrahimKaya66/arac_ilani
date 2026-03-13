namespace AracIlan.Sozlesmeler.Yanitlar;

/// <summary>
/// Motor seçeneği yanıtı - AI teknik özellik dolduracak
/// </summary>
public record MotorSecenegiYaniti(
    int Id,
    string Ad,
    int? MotorHacmi,
    string YakitTipi,
    int? Guc
);
