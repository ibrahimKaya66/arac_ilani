# Araç İlan API - Kurulum ve Kullanım

## Gereksinimler
- .NET 10 SDK
- SQL Server (LocalDB, Express veya tam sürüm)

## Veritabanı Bağlantısı

`appsettings.json` içinde connection string'i düzenleyin:

```json
{
  "ConnectionStrings": {
    "AracIlanVeritabani": "Server=(localdb)\\mssqllocaldb;Database=AracIlan;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Alternatif bağlantı örnekleri:**
- SQL Server Express: `Server=.\\SQLEXPRESS;Database=AracIlan;Trusted_Connection=True;`
- Tam sunucu: `Server=localhost;Database=AracIlan;User Id=sa;Password=***;TrustServerCertificate=True;`

## Kurulum Adımları

```bash
# 1. Proje dizinine git
cd src

# 2. Migration uygula (veritabanı oluşturulur)
dotnet ef database update --project AracIlan.Altyapi --startup-project AracIlan.Api

# 3. API'yi çalıştır
cd AracIlan.Api
dotnet run
```

API varsayılan olarak `https://localhost:7xxx` adresinde çalışır.
Swagger UI: `https://localhost:7xxx/swagger`

## API Endpoint'leri

### Kimlik (Auth)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/kimlik/kayit` | Kayıt - Body: { email, sifre, ad, soyad, telefon? } |
| POST | `/api/kimlik/giris` | Giriş - Body: { email, sifre } → JWT token döner |

### Marka/Model (Public)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/markalar?kategori=1` | Kategoriye göre markalar |
| GET | `/api/markalar/{markaId}/modeller` | Markaya ait modeller |
| GET | `/api/modeller/{modelId}/paketler?yil=2022` | Yıla göre paketler |
| GET | `/api/paketler/{paketId}/motor-secenekleri` | Motor seçenekleri |

### İlanlar
| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| GET | `/api/ilanlar` | - | Filtreli ilan listesi |
| GET | `/api/ilanlar/{id}` | - | İlan detayı |
| GET | `/api/ilanlar/hakkim` | ✓ | İlan hakkı bilgim |
| POST | `/api/ilanlar` | ✓ | Yeni ilan oluştur |

### Dosya Yükleme (Auth gerekli)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/gorseller/arac` | Araç fotoğrafı (form-data: dosya) |
| POST | `/api/gorseller/expertiz` | Expertiz görseli |

### AI
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/ai/teknik-ozellik?motorId=&uretimYili=` | Motor seçildiğinde teknik özellik üretir |

### Raporlar (Sadece Admin)
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/raporlar/en-cok-ilanli-markalar?adet=10` | En çok ilan verilen markalar |
| GET | `/api/raporlar/en-cok-ilanli-modeller?adet=10` | En çok ilan verilen modeller |
| GET | `/api/raporlar/kategori-dagilimi` | Otomobil/SUV/Pickup dağılımı |
| GET | `/api/raporlar/son-gunler?gun=30` | Son X günde ilan sayısı |

## Akıllı Form Akışı (İlan Verme)

1. `GET /api/markalar?kategori=1` → Marka seç
2. `GET /api/markalar/1/modeller` → Model seç
3. `GET /api/modeller/1/paketler?yil=2022` → Yıl gir, paket seç
4. `GET /api/paketler/1/motor-secenekleri` → Motor seç
5. `POST /api/ilanlar` → İlan verilerini gönder

## Google ile Giriş (Opsiyonel)

"Google ile Kayıt Ol" için OAuth kimlik bilgileri gerekir. Detay: [GOOGLE_OAUTH_KURULUM.md](GOOGLE_OAUTH_KURULUM.md)

## AI (Opsiyonel)

Teknik özellik ve expertiz analizi için **Groq (ücretsiz)** veya OpenAI kullanılır. Detay: [AI_API_KURULUM.md](AI_API_KURULUM.md)

**Groq (önerilen):** [console.groq.com](https://console.groq.com) → API Key al → `appsettings.json`:
```json
"AI": { "Provider": "Groq", "Groq": { "ApiKey": "gsk_xxx" } }
```

## Veri Tohumu

Geliştirme ortamında API ilk çalıştığında otomatik olarak örnek veri eklenir:
- Toyota Corolla (2020-2024)
- BMW 3 Serisi (2019-2024)
- Standart ve Premium üyelik paketleri
