# Araç İlanı v2.0.0

## Yeni Özellikler

### Filtreleme
- **İlanlar sayfası** (`/ilanlar`): Kategori, marka, model, yıl, fiyat, km ve sıralama filtreleri
- **Benim ilanlarım**: Aynı filtreleme desteği

### İlan Yönetimi
- **İlan silme**: Benim ilanlarım sayfası ve ilan detay sayfasından ilan silme
- Silinen ilanlara ait fotoğraf ve video dosyaları otomatik temizlenir

### Video Desteği
- Araç ilanlarına video ekleme ve silme
- Fotoğraf gibi video yükleme; silindiğinde dosya da kaldırılır

### Veritabanı
- **100 araç seed**: Gerçek fotoğraflarla 100 örnek araç verisi
- Development ortamında otomatik seed (`POST /api/seed/araclar`)

### Kimlik Doğrulama
- **JWT**: Token süresi 1 saat, sitede iken 15 dakikada bir otomatik yenileme
- `/ilanlar` sayfasına giriş yapmadan erişim (401 redirect düzeltmesi)

### Diğer İyileştirmeler
- Marka ve Model select'lerinde A–Z sıralama
- Paket güncellemeleri (Node.js ve NuGet)

## Teknik Detaylar

- **Backend**: .NET 10, Entity Framework Core 10, JWT Bearer
- **Frontend**: Next.js 16, React 19, TanStack Query, Zustand, Tailwind CSS 4
