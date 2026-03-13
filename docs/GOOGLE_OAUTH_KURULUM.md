# Google OAuth Kurulumu

"Google ile Kayıt Ol" özelliği için Google Cloud Console'da OAuth 2.0 kimlik bilgileri oluşturmanız gerekir.

## Adımlar

### 1. Google Cloud Console'a gidin
[https://console.cloud.google.com](https://console.cloud.google.com)

### 2. Proje oluşturun veya seçin
- Sol üstten **Proje seç** → **Yeni proje**
- Proje adı: örn. "AracIlan"
- **Oluştur** tıklayın

### 3. OAuth consent screen (izin ekranı)
- Sol menü: **API ve Hizmetler** → **OAuth consent screen**
- **External** seçin → **Oluştur**
- Uygulama adı: **Araç İlan**
- Kullanıcı destek e-postası: kendi e-postanız
- Geliştirici iletişim bilgileri: e-postanız
- **Kaydet ve devam** → **Kaydet ve devam** (kapsamlar opsiyonel) → **Ana sayfaya dön**

### 4. OAuth 2.0 kimlik bilgileri
- Sol menü: **API ve Hizmetler** → **Kimlik bilgileri**
- **+ Kimlik bilgisi oluştur** → **OAuth istemci kimliği**
- Uygulama türü: **Web uygulaması**
- Ad: **AracIlan Web**

**Yetkili JavaScript kaynakları:**
```
http://localhost:3000
```

**Yetkili yönlendirme URI'leri:**
```
http://localhost:5094/api/kimlik/google/callback
```

> Production için kendi domain'inizi ekleyin (örn. `https://aracilani.com`, `https://aracilani.com/auth/callback`)

- **Oluştur** tıklayın
- **Client ID** ve **Client Secret** değerlerini kopyalayın

### 5. appsettings.Development.json'a ekleyin

`src/AracIlan.Api/appsettings.Development.json` dosyasında:

```json
{
  "Google": {
    "ClientId": "123456789-xxxxxxxxxx.apps.googleusercontent.com",
    "ClientSecret": "GOCSPX-xxxxxxxxxxxxxxxxxxxx"
  },
  ...
}
```

Placeholder değerleri kendi kimlik bilgilerinizle değiştirin.

### 6. API'yi yeniden başlatın

Değişikliklerin uygulanması için API'yi durdurup tekrar çalıştırın.

---

## Test

1. `http://localhost:3000/kayit` sayfasına gidin
2. **Google ile Kayıt Ol** butonuna tıklayın
3. Google hesabınızla giriş yapın
4. Uygulama sizi geri yönlendirip otomatik giriş yapacaktır
