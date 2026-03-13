using AracIlan.Uygulama.Servisler;
using Microsoft.Extensions.DependencyInjection;

namespace AracIlan.Uygulama;

/// <summary>
/// Uygulama katmanı DI kayıtları
/// </summary>
public static class UygulamaUzantilari
{
    public static IServiceCollection UygulamaEkle(this IServiceCollection services)
    {
        services.AddScoped<MarkaServisi>();
        services.AddScoped<ModelServisi>();
        services.AddScoped<ModelPaketiServisi>();
        services.AddScoped<MotorSecenegiServisi>();
        services.AddScoped<IIlanServisi, IlanServisi>();

        return services;
    }
}
