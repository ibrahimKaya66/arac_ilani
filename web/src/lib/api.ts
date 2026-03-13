/**
 * API istemcisi - Backend AracIlan.Api ile iletişim
 */

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094";

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

  const res = await fetch(`${API_URL}${url}`, { ...init, headers });
  if (!res.ok) {
    const err = await res.json().catch(() => ({ message: res.statusText }));
    const body = err as { hata?: string; message?: string };
    throw new Error(body.hata ?? body.message ?? res.statusText ?? "API hatası");
  }
  return res.json();
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
  gorselYukle: async (dosya: File, token: string): Promise<string> => {
    const form = new FormData();
    form.append("dosya", dosya);
    const res = await fetch(`${API_URL}/api/gorseller/arac`, {
      method: "POST",
      headers: { Authorization: `Bearer ${token}` },
      body: form,
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({ hata: res.statusText }));
      throw new Error((err as { hata?: string }).hata ?? "Yükleme hatası");
    }
    const json = await res.json();
    return (json as { yol: string }).yol;
  },
  expertizGorseliYukle: async (dosya: File, token: string): Promise<string> => {
    const form = new FormData();
    form.append("dosya", dosya);
    const res = await fetch(`${API_URL}/api/gorseller/expertiz`, {
      method: "POST",
      headers: { Authorization: `Bearer ${token}` },
      body: form,
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({ hata: res.statusText }));
      throw new Error((err as { hata?: string }).hata ?? "Yükleme hatası");
    }
    const json = await res.json();
    return (json as { yol: string }).yol;
  },
  kimlik: {
    kayit: (data: { email: string; sifre: string; ad: string; soyad: string; telefon?: string }) =>
      fetcher<GirisYaniti>("/api/kimlik/kayit", { method: "POST", body: JSON.stringify(data) }),
    giris: (data: { email: string; sifre: string }) =>
      fetcher<GirisYaniti>("/api/kimlik/giris", { method: "POST", body: JSON.stringify(data) }),
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
