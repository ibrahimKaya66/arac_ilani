# AI API Kurulumu (Ücretsiz veya Ücretli)

Proje teknik özellik üretimi ve expertiz görsel analizi için AI kullanır. **Groq ücretsiz**, OpenAI ücretlidir.

---

## Ücretsiz: Groq (Önerilen)

**Günlük 14.400 istek, kredi kartı gerekmez.**

1. [console.groq.com](https://console.groq.com) adresine git
2. Google ile giriş yap
3. **API Keys** → **Create API Key** → Key'i kopyala
4. `appsettings.json` içinde:

```json
"AI": {
  "Provider": "Groq",
  "Groq": {
    "ApiKey": "gsk_xxxxxxxxxxxx"
  },
  "OpenAI": {
    "ApiKey": ""
  }
}
```

Veya environment variable: `AI__Groq__ApiKey=gsk_xxx`

---

## Ücretli: OpenAI

Yeni hesaplara $5 kredi verilir. [platform.openai.com](https://platform.openai.com) → API Keys.

```json
"AI": {
  "Provider": "OpenAI",
  "Groq": { "ApiKey": "" },
  "OpenAI": {
    "ApiKey": "sk-xxxxxxxxxxxx"
  }
}
```

---

## Özet

| Sağlayıcı | Limit | Maliyet |
|-----------|-------|---------|
| **Groq** | 14.400 istek/gün | Ücretsiz |
| **OpenAI** | Krediye göre | ~$5 başlangıç kredisi |

Key yoksa AI özellikleri devre dışı kalır; ilan verme normal çalışır.
