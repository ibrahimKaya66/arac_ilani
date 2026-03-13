using System.Text;
using AracIlan.Altyapi;
using AracIlan.Altyapi.Veritabani;
using AracIlan.Uygulama;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AltyapiEkle(builder.Configuration);
builder.Services.UygulamaEkle();

builder.Services.AddCors(opts =>
    opts.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// JWT + Google (Google sadece ClientId yapılandırıldıysa eklenir)
var jwtAnahtar = builder.Configuration["Jwt:Anahtar"] ?? "AracIlan-Gizli-Anahtar-En-Az-32-Karakter-Olmali-2024";
var authBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Yayinci"],
            ValidAudience = builder.Configuration["Jwt:HedefKitle"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAnahtar))
        };
    })
    .AddCookie("ExternalCookie");

var googleClientId = builder.Configuration["Google:ClientId"];
if (!string.IsNullOrWhiteSpace(googleClientId))
{
    authBuilder.AddGoogle(options =>
    {
        options.SignInScheme = "ExternalCookie";
        options.ClientId = googleClientId;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? "";
        options.CallbackPath = "/api/kimlik/google/callback";
    });
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Araç İlan API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

// API rotaları için JSON hata yanıtı (Development'ta HTML yerine)
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var ex = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        var hata = ex?.Message ?? "Beklenmeyen bir hata oluştu.";
        await context.Response.WriteAsJsonAsync(new { hata });
    });
});

// Veri tohumu
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AracIlanVeritabani>();
    try
    {
        await VeriTohumu.TohumlaAsync(db, scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogWarning(ex, "Veri tohumu atılamadı");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Uploads klasörü statik dosya olarak sunulur
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.MapControllers();

app.Run();
