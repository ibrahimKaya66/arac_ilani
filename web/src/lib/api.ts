/**
 * API istemcisi - Backend AracIlan.Api ile iletişim
 */

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094";

/** Token yenile - 401'de veya süre dolmadan önce kullanılır */
export async function tokenYenile(mevcutToken: string): Promise<GirisYaniti | null> {
  try {
    const res = await fetch(`${API_URL}/api/kimlik/refresh`, {
      method: "POST",
      headers: { Authorization: `Bearer ${mevcutToken}` },
    });
    if (!res.ok) return null;
    const text = await res.text();
    return (text.trim() ? JSON.parse(text) : null) as GirisYaniti;
  } catch {
    return null;
  }
}

/** JWT exp claim'ini (saniye) döndürür */
export function jwtExpAl(token: string): number | null {
  try {
    const parts = token.split(".");
    if (parts.length !== 3) return null;
    const payload = JSON.parse(atob(parts[1].replace(/-/g, "+").replace(/_/g, "/")));
    return payload.exp ?? null;
  } catch {
    return null;
  }
}

async function fetcher<T>(
  url: string,
  options?: RequestInit & { token?: string }
): Promise<T> {
  const { token, ...init } = options ?? {};
  const headers: HeadersInit = {
    "Content-Type": "application/json",
    ...init.headers,
  };
  if (token) {
    (headers as Record<string, string>)["Authorization"] = `Bearer ${token}`;
  }

  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), 15_000);
  let res = await fetch(`${API_URL}${url}`, { ...init, headers, signal: controller.signal }).finally(() => clearTimeout(timeout));

  if (res.status === 401 && token) {
    const refreshed = await tokenYenile(token);
    if (refreshed && typeof window !== "undefined") {
      const { useAuthStore } = await import("@/lib/auth-store");
      useAuthStore.getState().girisYap(refreshed.token, refreshed.kullaniciId, refreshed.ad, refreshed.soyad, refreshed.email, refreshed.roller, true);
      const retryController = new AbortController();
      const retryTimeout = setTimeout(() => retryController.abort(), 15_000);
      res = await fetch(`${API_URL}${url}`, {
        ...init,
        headers: { ...headers, Authorization: `Bearer ${refreshed.token}` },
        signal: retryController.signal,
      }).finally(() => clearTimeout(retryTimeout));
    }
  }

  if (!res.ok) {
    const err = await res.json().catch(() => ({ message: res.statusText }));
    const body = err as { hata?: string; message?: string; title?: string; errors?: Record<string, string[]> };
    let msg = body.hata ?? body.message ?? body.title ?? res.statusText ?? "API hatası";
    if (body.errors && typeof body.errors === "object") {
      const errList = Object.values(body.errors).flat().filter(Boolean);
      if (errList.length) msg = errList.join(" ");
    }
    if (res.status === 405) {
      throw new Error(`${msg} - API'yi yeniden başlatıp tekrar deneyin.`);
    }
    if (res.status === 401) {
      if (typeof window !== "undefined") {
        const { useAuthStore } = await import("@/lib/auth-store");
        useAuthStore.getState().cikisYap();
        window.location.href = "/giris";
      }
      throw new Error("Oturum süresi doldu. Tekrar giriş yapın.");
    }
    throw new Error(msg);
  }
  const text = await res.text();
  return (text.trim() ? JSON.parse(text) : null) as T;
}

export const api = {
  markalar: (kategori: number) => fetcher<{ id: number; ad: string; slug: string; sira: number }[]>(`/api/markalar?kategori=${kategori}`),
  modeller: (markaId: number) => fetcher<{ id: number; ad: string; slug: string; uretimBaslangicYili: number; uretimBitisYili: number }[]>(`/api/markalar/${markaId}/modeller`),
  paketler: (modelId: number, yil: number) => fetcher<{ id: number; ad: string; baslangicYili: number; bitisYili: number }[]>(`/api/modeller/${modelId}/paketler?yil=${yil}`),
  motorSecenekleri: (paketId: number) => fetcher<{ id: number; ad: string; motorHacmi: number | null; yakitTipi: string; guc: number | null }[]>(`/api/paketler/${paketId}/motor-secenekleri`),
  ilanlar: (params: Record<string, string | number | undefined>) => {
    const q = new URLSearchParams();
    Object.entries(params).forEach(([k, v]) => v != null && q.set(k, String(v)));
    return fetcher<{ ilanlar: IlanListe[]; toplamKayit: number; sayfa: number; sayfaBoyutu: number }>(`/api/ilanlar?${q}`);
  },
  ilanDetay: (id: number) => fetcher<IlanDetay>(`/api/ilanlar/${id}`),
  ilanYayinla: (id: number, token: string) =>
    fetcher<void>(`/api/ilanlar/${id}/yayinla`, { method: "POST", token }),
  ilanSatildi: (id: number, token: string) =>
    fetcher<void>(`/api/ilanlar/${id}/satildi`, { method: "POST", token }),
  ilanlarim: (sayfa: number, sayfaBoyutu: number, token: string) =>
    fetcher<{ ilanlar: IlanListe[]; toplamKayit: number; sayfa: number; sayfaBoyutu: number }>(
      `/api/ilanlar/benim?sayfa=${sayfa}&sayfaBoyutu=${sayfaBoyutu}`,
      { token }
    ),
  ilanOlustur: (data: IlanOlusturmaIstegi, token: string) =>
    fetcher<{ id: number }>("/api/ilanlar", { method: "POST", body: JSON.stringify(data), token }),
  ilanGuncelle: (id: number, data: IlanGuncellemeIstegi, token: string) =>
    fetcher<void>(`/api/ilanlar/${id}`, { method: "PUT", body: JSON.stringify(data), token }),
  ilanHakkim: (token: string) =>
    fetcher<{ yeterli: boolean; kalan: number; maksFotograf: number; ilanSuresiGun: number }>("/api/ilanlar/hakkim", { token }),
  gorselYukle: async (dosya: File, token: string): Promise<string> => {
    let currentToken = token;
    const doUpload = async () => {
      const form = new FormData();
      form.append("dosya", dosya);
      const res = await fetch(`${API_URL}/api/gorseller/arac`, {
        method: "POST",
        headers: { Authorization: `Bearer ${currentToken}` },
        body: form,
      });
      if (res.status === 401) {
        const refreshed = await tokenYenile(currentToken);
        if (refreshed && typeof window !== "undefined") {
          const { useAuthStore } = await import("@/lib/auth-store");
          useAuthStore.getState().girisYap(refreshed.token, refreshed.kullaniciId, refreshed.ad, refreshed.soyad, refreshed.email, refreshed.roller, true);
          currentToken = refreshed.token;
          return doUpload();
        }
      }
      if (!res.ok) {
        const err = await res.json().catch(() => ({ hata: res.statusText }));
        throw new Error((err as { hata?: string }).hata ?? "Yükleme hatası");
      }
      const json = await res.json();
      return (json as { yol: string }).yol;
    };
    return doUpload();
  },
  expertizGorseliYukle: async (dosya: File, token: string): Promise<string> => {
    let currentToken = token;
    const doUpload = async () => {
      const form = new FormData();
      form.append("dosya", dosya);
      const res = await fetch(`${API_URL}/api/gorseller/expertiz`, {
        method: "POST",
        headers: { Authorization: `Bearer ${currentToken}` },
        body: form,
      });
      if (res.status === 401) {
        const refreshed = await tokenYenile(currentToken);
        if (refreshed && typeof window !== "undefined") {
          const { useAuthStore } = await import("@/lib/auth-store");
          useAuthStore.getState().girisYap(refreshed.token, refreshed.kullaniciId, refreshed.ad, refreshed.soyad, refreshed.email, refreshed.roller, true);
          currentToken = refreshed.token;
          return doUpload();
        }
      }
      if (!res.ok) {
        const err = await res.json().catch(() => ({ hata: res.statusText }));
        throw new Error((err as { hata?: string }).hata ?? "Yükleme hatası");
      }
      const json = await res.json();
      return (json as { yol: string }).yol;
    };
    return doUpload();
  },
  kimlik: {
    kayit: (data: { email: string; sifre: string; ad: string; soyad: string; telefon?: string }) =>
      fetcher<GirisYaniti>("/api/kimlik/kayit", { method: "POST", body: JSON.stringify(data) }),
    giris: (data: { email: string; sifre: string }) =>
      fetcher<GirisYaniti>("/api/kimlik/giris", { method: "POST", body: JSON.stringify(data) }),
    refresh: (mevcutToken: string) => tokenYenile(mevcutToken),
  },
  raporlar: {
    tarihAraligi: (baslangic: string, bitis: string, token: string) =>
      fetcher<TarihAraligiRaporu>(`/api/raporlar/tarih-araligi?baslangic=${baslangic}&bitis=${bitis}`, { token }),
    uretimYiliSatis: (baslangic: string, bitis: string, token: string) =>
      fetcher<UretimYiliSatisRaporu[]>(`/api/raporlar/uretim-yili-satis?baslangic=${baslangic}&bitis=${bitis}`, { token }),
    enHizliSatilanlar: (adet: number, token: string) =>
      fetcher<HizliSatisRaporu[]>(`/api/raporlar/en-hizli-satilanlar?adet=${adet}`, { token }),
  },
};

export interface TarihAraligiRaporu {
  toplamSatis: number;
  toplamCiro: number;
  aktifIlanSayisi: number;
}

export interface UretimYiliSatisRaporu {
  uretimYili: number;
  satisSayisi: number;
  toplamCiro: number;
}

export interface HizliSatisRaporu {
  ilanId: number;
  baslik: string;
  satisSuresiGun: number;
  fiyat: number;
}

export interface IlanListe {
  id: number;
  baslik: string;
  uretimYili: number;
  kilometre: number;
  fiyat: number;
  kapakGorselYolu: string;
  markaAd: string;
  modelAd: string;
  hasarDurumu: string;
}

export interface IlanDetay {
  id: number;
  baslik: string;
  aciklama: string;
  kategori: string;
  uretimYili: number;
  kilometre: number;
  fiyat: number;
  renk: string;
  vitesTipi: string;
  hasarDurumu: string;
  markaAd: string;
  modelAd: string;
  motorAd: string;
  gorselYollari: string[];
  teknikOzelliklerJson: string | null;
  expertizAIAnalizi: string | null;
  ilanDurumu: string;
  kullaniciId: string | null;
}

export interface IlanGuncellemeIstegi {
  kilometre?: number;
  fiyat?: number;
  renk?: string;
  vitesTipi?: string;
  aciklama?: string;
  hasarDurumu?: number;
  gorselYollari?: string[];
  expertizGorselYolu?: string;
}

export interface IlanOlusturmaIstegi {
  kategori: number;
  motorSecenegiId: number;
  uretimYili: number;
  kilometre: number;
  fiyat: number;
  renk: string;
  vitesTipi: string;
  aciklama: string;
  hasarDurumu: number;
  gorselYollari?: string[];
  expertizGorselYolu?: string;
}

export interface GirisYaniti {
  token: string;
  gecerlilikTarihi: string;
  kullaniciId: string;
  email: string;
  ad: string;
  soyad: string;
  roller: string[];
}
