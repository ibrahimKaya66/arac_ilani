using AracIlan.Alan.Varliklar;
using AracIlan.Altyapi.Kimlik;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AracIlan.Altyapi.Veritabani;

/// <summary>
/// Ana veritabanı bağlamı - Identity + tüm varlıklar
/// Tablo adları Türkçe.
/// </summary>
public class AracIlanVeritabani(DbContextOptions<AracIlanVeritabani> options)
    : IdentityDbContext<Kullanici>(options)
{
    public DbSet<Marka> Markalar => Set<Marka>();
    public DbSet<Model> Modeller => Set<Model>();
    public DbSet<ModelPaketi> ModelPaketleri => Set<ModelPaketi>();
    public DbSet<MotorSecenegi> MotorSecenekleri => Set<MotorSecenegi>();
    public DbSet<Arac> Araclar => Set<Arac>();
    public DbSet<AracGorseli> AracGorselleri => Set<AracGorseli>();
    public DbSet<AracVideosu> AracVideolari => Set<AracVideosu>();
    public DbSet<ExpertizRaporu> ExpertizRaporlari => Set<ExpertizRaporu>();
    public DbSet<UyelikPaketi> UyelikPaketleri => Set<UyelikPaketi>();
    public DbSet<KullaniciAboneligi> KullaniciAbonelikleri => Set<KullaniciAboneligi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Identity tabloları Türkçe
        modelBuilder.Entity<Kullanici>().ToTable("Kullanicilar");

        // Tablo adları Türkçe
        modelBuilder.Entity<Marka>().ToTable("Markalar");
        modelBuilder.Entity<Model>().ToTable("Modeller");
        modelBuilder.Entity<ModelPaketi>().ToTable("ModelPaketleri");
        modelBuilder.Entity<MotorSecenegi>().ToTable("MotorSecenekleri");
        modelBuilder.Entity<Arac>().ToTable("Araclar");
        modelBuilder.Entity<AracGorseli>().ToTable("AracGorselleri");
        modelBuilder.Entity<AracVideosu>().ToTable("AracVideolari");
        modelBuilder.Entity<ExpertizRaporu>().ToTable("ExpertizRaporlari");
        modelBuilder.Entity<UyelikPaketi>().ToTable("UyelikPaketleri");
        modelBuilder.Entity<KullaniciAboneligi>().ToTable("KullaniciAbonelikleri");

        // Soft delete filtresi
        modelBuilder.Entity<Marka>().HasQueryFilter(m => !m.Silindi);
        modelBuilder.Entity<Model>().HasQueryFilter(m => !m.Silindi);
        modelBuilder.Entity<ModelPaketi>().HasQueryFilter(p => !p.Silindi);
        modelBuilder.Entity<MotorSecenegi>().HasQueryFilter(m => !m.Silindi);
        modelBuilder.Entity<Arac>().HasQueryFilter(a => !a.Silindi);
        modelBuilder.Entity<AracGorseli>().HasQueryFilter(g => !g.Silindi);
        modelBuilder.Entity<AracVideosu>().HasQueryFilter(v => !v.Silindi);
        modelBuilder.Entity<ExpertizRaporu>().HasQueryFilter(e => !e.Silindi);
        modelBuilder.Entity<UyelikPaketi>().HasQueryFilter(u => !u.Silindi);
        modelBuilder.Entity<KullaniciAboneligi>().HasQueryFilter(k => !k.Silindi);

        // İlişkiler - Cascade: silme sırasında parent silinince child'lar da silinsin (test verisi temizliği için)
        modelBuilder.Entity<Model>()
            .HasOne(m => m.Marka)
            .WithMany(m => m.Modeller)
            .HasForeignKey(m => m.MarkaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModelPaketi>()
            .HasOne(p => p.Model)
            .WithMany(m => m.Paketler)
            .HasForeignKey(p => p.ModelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MotorSecenegi>()
            .HasOne(m => m.ModelPaketi)
            .WithMany(p => p.MotorSecenekleri)
            .HasForeignKey(m => m.ModelPaketiId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Arac>()
            .HasOne(a => a.MotorSecenegi)
            .WithMany()
            .HasForeignKey(a => a.MotorSecenegiId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AracGorseli>()
            .HasOne(g => g.Arac)
            .WithMany(a => a.Gorseller)
            .HasForeignKey(g => g.AracId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AracVideosu>()
            .HasOne(v => v.Arac)
            .WithMany(a => a.Videolar)
            .HasForeignKey(v => v.AracId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExpertizRaporu>()
            .HasOne(e => e.Arac)
            .WithOne(a => a.ExpertizRaporu)
            .HasForeignKey<ExpertizRaporu>(e => e.AracId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KullaniciAboneligi>()
            .HasOne(k => k.UyelikPaketi)
            .WithMany()
            .HasForeignKey(k => k.UyelikPaketiId)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision - TL fiyatları için
        modelBuilder.Entity<Arac>().Property(a => a.Fiyat).HasPrecision(18, 2);
        modelBuilder.Entity<UyelikPaketi>().Property(u => u.Fiyat).HasPrecision(18, 2);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<TemelVarlik>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.OlusturmaTarihi = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.GuncellemeTarihi = DateTime.UtcNow;
                if (entry.Entity.Silindi)
                {
                    entry.Entity.SilinmeTarihi = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
