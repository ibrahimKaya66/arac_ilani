namespace AracIlan.Alan.Sabitler;

/// <summary>
/// Araç kategorisi - Sadece Otomobil, SUV ve Pickup kabul edilir.
/// Motosiklet, kamyon vb. bu platformda yer almaz.
/// </summary>
public enum AracKategorisi
{
    /// <summary>Binek otomobil</summary>
    Otomobil = 1,

    /// <summary>Sport Utility Vehicle</summary>
    SUV = 2,

    /// <summary>Pickup kamyonet</summary>
    Pickup = 3
}
