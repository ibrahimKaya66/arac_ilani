using AracIlan.Altyapi.Depolar;
using AracIlan.Altyapi.Kimlik;
using AracIlan.Altyapi.Servisler;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama.Depolar;
using AracIlan.Uygulama.Servisler;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AracIlan.Altyapi;

/// <summary>
/// Altyapı katmanı DI kayıtları
/// </summary>
public static class AltyapiUzantilari
{
    public static IServiceCollection AltyapiEkle(this IServiceCollection services, IConfiguration yapilandirma)
    {
        var baglanti = yapilandirma.GetConnectionString("AracIlanVeritabani")
            ?? "Server=(localdb)\\mssqllocaldb;Database=AracIlan;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<AracIlanVeritabani>(opts => opts.UseSqlServer(baglanti));

        services.AddIdentity<Kullanici, Microsoft.AspNetCore.Identity.IdentityRole>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireUppercase = true;
            opts.Password.RequireNonAlphanumeric = true;
            opts.Password.RequiredLength = 6;
            opts.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AracIlanVeritabani>()
        .AddRoles<Microsoft.AspNetCore.Identity.IdentityRole>()
        .AddDefaultTokenProviders();

        services.AddScoped<IMarkaDeposu, MarkaDeposu>();
        services.AddScoped<IModelDeposu, ModelDeposu>();
        services.AddScoped<IModelPaketiDeposu, ModelPaketiDeposu>();
        services.AddScoped<IMotorSecenegiDeposu, MotorSecenegiDeposu>();
        services.AddScoped<IAracDeposu, AracDeposu>();
        services.AddScoped<IKullaniciAbonelikDeposu, KullaniciAbonelikDeposu>();
        services.AddScoped<IDosyaServisi, DosyaServisi>();
        services.AddScoped<IAIServisi, AIServisi>();
        services.AddScoped<IRaporDeposu, RaporDeposu>();

        return services;
    }
}
