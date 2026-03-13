# Araç İlan Platformu - Proje Planı

## 🎯 Proje Hedefi
Türkiye'nin en iyi araç alım-satım platformu. Sadece araç ilanları (Otomobil, SUV, Pickup).

---

## 🏗️ Mimari Seçimi: Temiz Mimari (Clean Architecture)

**Neden Temiz Mimari?**
- **Bağımsızlık:** Alan katmanı hiçbir dış bağımlılığa sahip değil
- **Test edilebilirlik:** İş kuralları izole test edilebilir
- **Bakım kolaylığı:** Değişiklikler tek bir yerde yapılır
- **Ölçeklenebilirlik:** Yeni özellikler mevcut kodu bozmadan eklenir
- **Ekip uyumu:** Katmanlar net, her geliştirici sorumluluk alanını bilir

**Katman Sorumlulukları:**
| Katman | Sorumluluk |
|--------|-------------|
| Alan | Varlıklar, iş kuralları, enum'lar — hiçbir dış referans yok |
| Uygulama | Use case'ler, servisler, DTO'lar — sadece Alan'a bağımlı |
| Altyapı | EF, API çağrıları, dosya — Uygulama arayüzlerini implement eder |
| Api | Controller'lar, middleware — sadece Uygulama'yı kullanır |

---

## 🏗️ Teknoloji Seçimleri

### Backend
| Teknoloji | Seçim | Gerekçe |
|-----------|-------|---------|
| Framework | **ASP.NET Core 10** | En güncel LTS, MSSQL native desteği, yüksek performans |
| Mimari | **Temiz Mimari (Clean Architecture)** | Test edilebilir, bakımı kolay, katmanlı yapı |
| ORM | **Entity Framework Core 10** | MSSQL ile mükemmel entegrasyon |
| Auth | **ASP.NET Core Identity + JWT** | Güvenli, ölçeklenebilir kimlik doğrulama |

### Frontend
| Teknoloji | Seçim | Gerekçe |
|-----------|-------|---------|
| Framework | **Next.js 14 (App Router)** | SEO için SSR/SSG, React ekosistemi |
| Dil | **TypeScript** | Tip güvenliği, hata önleme |
| UI | **Tailwind CSS + shadcn/ui** | Modern, tutarlı, özelleştirilebilir |
| State | **Zustand + React Query** | Hafif, performanslı state yönetimi |

### Veritabanı
| Teknoloji | Seçim |
|-----------|-------|
| DB | **Microsoft SQL Server** |
| Migrations | **EF Core Migrations** |

### AI & Ek Servisler
| Özellik | Teknoloji |
|---------|-----------|
| Expertiz görsel analizi | **OpenAI GPT-4 Vision API** |
| Teknik özellik doldurma | **OpenAI GPT-4** |
| Ödeme (Türkiye) | **Iyzico** |
| Dosya depolama | **Azure Blob / AWS S3** veya lokal |

### Mobil (Faz 2)
| Teknoloji | Seçim |
|-----------|-------|
| Framework | **React Native (Expo)** |
| Paylaşım | API ortak, UI native |

---

## 📁 Proje Yapısı

### Katman Adları (Türkçe)

```
cursor-app/
├── src/
│   ├── AracIlan.Api/                 # Web API - Giriş noktası
│   ├── AracIlan.Uygulama/            # İş kuralları, Servisler, DTO'lar
│   ├── AracIlan.Alan/                # Varlıklar, Alan mantığı, Sabitler
│   ├── AracIlan.Altyapi/             # EF, Harici servisler, Veri erişimi
│   └── AracIlan.Sozlesmeler/         # Paylaşılan DTO'lar, API sözleşmeleri
├── web/                              # Next.js frontend
├── docs/                             # Dokümantasyon
└── PROJECT_PLAN.md
```

### Katman Bağımlılıkları (İç → Dış)
```
Alan (Domain) ← Uygulama ← Altyapı
     ↑              ↑
     └──────────────┘
     Sozlesmeler (her katmandan referans alabilir)
```

---

## 📊 Veritabanı Şeması (Türkçe Tablo/Model Adları)

### Ana Tablolar
| Tablo Adı | Model Sınıfı | Açıklama |
|-----------|--------------|----------|
| **Kullanicilar** | Kullanici | Kullanıcılar (Identity ile) |
| **KullaniciRolleri** | KullaniciRolu | Admin, Bayi, Premium, Standart |
| **Markalar** | Marka | Toyota, BMW, vb. |
| **Modeller** | Model | Corolla, 3 Serisi, vb. |
| **ModelPaketleri** | ModelPaketi | Paketler (yıl bazlı) |
| **MotorSecenekleri** | MotorSecenegi | Motor seçenekleri (dönem bazlı) |
| **Araclar** | Arac | Araç ilanları |
| **AracGorselleri** | AracGorseli | Araç fotoğrafları |
| **ExpertizRaporlari** | ExpertizRaporu | Expertiz raporları (AI analizi) |
| **UyelikPaketleri** | UyelikPaketi | Üyelik paketleri |
| **KullaniciAbonelikleri** | KullaniciAboneligi | Kullanıcı abonelikleri |
| **Raporlar** | - | Raporlama view'ları |

### Alan Sabitleri (Enum'lar - Türkçe)
- **AracKategorisi**: Otomobil, SUV, Pickup
- **HasarDurumu**: Bilinmiyor, Hasarsız, HasarKayıtlı, ExpertizVar
- **IlanDurumu**: Taslak, Yayında, SüresiDoldu, Kaldırıldı

### Kategoriler (Sabit)
- Otomobil
- SUV
- Pickup

---

## 📝 İsimlendirme Kuralları (Tüm Proje Türkçe)

### Katman Projeleri
| İngilizce | Türkçe |
|-----------|--------|
| Api | AracIlan.Api |
| Application | AracIlan.Uygulama |
| Domain | AracIlan.Alan |
| Infrastructure | AracIlan.Altyapi |
| Contracts | AracIlan.Sozlesmeler |

### Sınıf/Model İsimleri
- **Varlıklar:** Kullanici, Marka, Model, Arac, MotorSecenegi
- **Servisler:** IlanServisi, MarkaServisi, AIServisi
- **Veri Depoları:** IAracDeposu, IMarkaDeposu (Repository pattern - Türkçe arayüz)
- **DTO'lar:** IlanOlusturmaIstegi, IlanDetayYaniti

### API Endpoint Örnekleri (Türkçe path)
- `GET /api/markalar`
- `GET /api/markalar/{id}/modeller`
- `GET /api/modeller/{id}/paketler?yil=2022`
- `GET /api/paketler/{id}/motor-secenekleri`
- `POST /api/ilanlar`
- `GET /api/ilanlar` (filtreleme parametreleri ile)

### Veritabanı
- Tablo adları: PascalCase, Türkçe (Markalar, Modeller, Araclar)
- Kolon adları: PascalCase, Türkçe (UretimYili, Kilometre, Fiyat)

---

## 🔐 Kullanıcı Rolleri

| Rol | Dashboard | İlan Hakkı | Fotoğraf | Süre |
|-----|-----------|------------|----------|------|
| **Admin** | Tüm sistem, raporlar | Sınırsız | Sınırsız | Sınırsız |
| **Dealer** | Bayi paneli | Pakete göre | Pakete göre | Pakete göre |
| **Premium** | Premium panel | 10 ilan | 24 foto | 60 gün |
| **Standard** | Standart panel | 3 ilan | 8 foto | 30 gün |

---

## 📱 Uygulama Akışı (İlan Verme)

1. **Marka seç** → Model listesi gelir
2. **Model seç** → Yıl aralığı gelir
3. **Yıl gir** → O yıla ait paketler gelir (otomatik)
4. **Paket seç** → Motor seçenekleri gelir (otomatik)
5. **Motor seç** → AI teknik özellikleri doldurur (otomatik)
6. **Expertiz yükle** (opsiyonel) → AI parça analizi yapar
7. **Fiyat, km, renk** → Manuel (validasyonlu)
8. **Fotoğraf yükle** → Paket limitine göre

---

## 📅 Geliştirme Fazları

### Faz 1: Temel Altyapı (Adım 1-3) ✅
- [x] Proje planı
- [x] Backend solution kurulumu (Temiz Mimari, Türkçe katmanlar)
- [x] Veritabanı modelleri (MSSQL, EF Core)
- [x] Temel API endpoints (Marka, Model, Paket, Motor, İlan)

### Faz 2: İlan Sistemi (Adım 4-6)
- [ ] İlan CRUD
- [ ] Akıllı form (yıl→paket→motor)
- [ ] AI teknik özellik entegrasyonu

### Faz 3: Kullanıcı & Ödeme (Adım 7-9)
- [ ] Auth & Roller
- [ ] Üyelik paketleri
- [ ] Iyzico entegrasyonu

### Faz 4: Expertiz & Raporlama (Adım 10-12)
- [ ] Expertiz AI analizi
- [ ] Admin raporları
- [ ] SEO optimizasyonu

### Faz 5: Frontend & Yayın (Adım 13-15)
- [ ] Next.js UI
- [ ] Mobil responsive
- [ ] Hosting & deployment

---

## 🇹🇷 Türkiye Hosting Önerisi (Proje Sonunda)

Değerlendirilecek firmalar:
- **Turhost** - Yerel, MSSQL desteği
- **Natro** - Türkiye lokasyonlu
- **Radore** - Enterprise
- **Azure Türkiye** - Microsoft, veri merkezi İstanbul

---

*Bu doküman proje boyunca güncellenecektir.*
