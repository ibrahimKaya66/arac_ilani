# Araç İlan

> Türkiye'nin araç alım satım platformu – Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım.

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Next.js](https://img.shields.io/badge/Next.js-16-000000?logo=next.js)](https://nextjs.org/)
[![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)](https://react.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## Özellikler

- **İlan Yönetimi** – Araç ilanı oluşturma, düzenleme, fotoğraf yükleme
- **Kimlik Doğrulama** – JWT, Google OAuth, Beni hatırla, Refresh token
- **Gmail Kısayolu** – `ibrahim.kaya5466` ile `ibrahim.kaya5466@gmail.com` adresine giriş
- **Marka/Model Kataloğu** – Otomobil, SUV, Pickup kategorileri
- **AI Destekli** – Teknik özellik üretimi, Expertiz görsel analizi (Groq/OpenAI)
- **Admin Panel** – Raporlar, satış istatistikleri
- **Responsive Tasarım** – Mobil uyumlu arayüz

---

## Teknoloji Yığını

| Katman | Teknolojiler |
|--------|--------------|
| **Backend** | .NET 10, ASP.NET Core, Entity Framework Core, SQL Server, JWT, Swagger |
| **Frontend** | Next.js 16, React 19, TypeScript, Tailwind CSS 4, TanStack Query, Zustand |
| **AI** | Groq / OpenAI (yapılandırılabilir) |

---

## Proje Yapısı

```
arac_ilani/
├── src/                    # .NET Backend
│   ├── AracIlan.Api/       # Web API (Controllers, Program.cs)
│   ├── AracIlan.Altyapi/   # EF Core, Identity, Veritabanı
│   ├── AracIlan.Alan/      # Domain modelleri
│   ├── AracIlan.Sozlesmeler/  # DTO'lar, İstek/Yanıt sözleşmeleri
│   └── AracIlan.Uygulama/  # İş mantığı, Servisler
├── web/                    # Next.js Frontend
│   └── src/app/            # Sayfalar, bileşenler
├── docs/                   # Dokümantasyon
└── .github/                # GitHub Actions, Releases
```

---

## Hızlı Başlangıç

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- SQL Server (LocalDB, Express veya tam sürüm)

### 1. Veritabanı Kurulumu

```bash
cd src
dotnet ef database update --project AracIlan.Altyapi --startup-project AracIlan.Api
```

### 2. API Başlatma

```bash
cd src/AracIlan.Api
dotnet run
```

API: `http://localhost:5094` | Swagger: `http://localhost:5094/swagger`

### 3. Frontend Başlatma

```bash
cd web
npm install
npm run dev
```

Web: `http://localhost:3000`

### 4. Yapılandırma

- **API:** `src/AracIlan.Api/appsettings.Development.json` (gizli anahtarlar)
- **Frontend:** `web/.env.local` → `NEXT_PUBLIC_API_URL=http://localhost:5094`

Detaylı kurulum: [docs/API_KURULUM.md](docs/API_KURULUM.md) | [docs/AI_API_KURULUM.md](docs/AI_API_KURULUM.md)

---

## GitHub

- **Releases:** `v1.0.0` gibi tag push'landığında otomatik release oluşturulur
- **Insights:** Repo Insights – Katkılar, trafik, bağımlılıklar otomatik takip edilir

---

## Lisans

MIT License – Detaylar için [LICENSE](LICENSE) dosyasına bakın.
