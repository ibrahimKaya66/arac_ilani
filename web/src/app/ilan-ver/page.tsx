"use client";

import { useEffect, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useMutation, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore, useAuthHydrated } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";

const KATEGORILER = [
  { deger: 1, ad: "Otomobil" },
  { deger: 2, ad: "SUV" },
  { deger: 3, ad: "Pickup" },
];

const RENKLER = [
  "Beyaz", "Siyah", "Gri", "Gümüş", "Kırmızı", "Mavi", "Yeşil", "Bej", "Kahverengi",
  "Turuncu", "Sarı", "Lacivert", "Bordo", "Antrasit", "Bronz", "Diğer",
];

export default function IlanVerPage() {
  const router = useRouter();
  const hydrated = useAuthHydrated();
  const { girisliMi, token } = useAuthStore(useShallow((s) => ({ girisliMi: s.girisliMi, token: s.token })));
  const [adim, setAdim] = useState(1);
  const [gorselYollari, setGorselYollari] = useState<string[]>([]);
  const [expertizYolu, setExpertizYolu] = useState<string | null>(null);
  const [hata, setHata] = useState<string | null>(null);
  const gorselInputRef = useRef<HTMLInputElement>(null);
  const expertizInputRef = useRef<HTMLInputElement>(null);
  const [form, setForm] = useState({
    kategori: 1,
    markaId: 0,
    modelId: 0,
    yil: new Date().getFullYear(),
    paketId: 0,
    motorId: 0,
    kilometre: 0,
    fiyat: 0,
    renk: "",
    vitesTipi: "Manuel",
    aciklama: "",
    hasarDurumu: 0,
  });

  useEffect(() => {
    if (!hydrated) return;
    const t = setTimeout(() => {
      if (!girisliMi) router.push("/giris");
    }, 150);
    return () => clearTimeout(t);
  }, [hydrated, girisliMi, router]);

  const { data: markalar, refetch: markalarYenile, isFetching: markalarYukleniyor } = useQuery({
    queryKey: ["markalar", form.kategori],
    queryFn: () => api.markalar(form.kategori),
  });

  // Markalar boşsa seed tetikle (DB'de yoksa ekler)
  const seedTetiklenen = useRef(false);
  useEffect(() => {
    if (!markalarYukleniyor && markalar?.length === 0 && form.kategori && !seedTetiklenen.current) {
      seedTetiklenen.current = true;
      fetch(`${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094"}/api/seed`, { method: "POST" })
        .then((r) => { if (r.ok) markalarYenile(); });
    }
  }, [markalarYukleniyor, markalar?.length, form.kategori, markalarYenile]);

  const { data: modeller } = useQuery({
    queryKey: ["modeller", form.markaId],
    queryFn: () => api.modeller(form.markaId),
    enabled: form.markaId > 0,
  });

  const secilenModel = modeller?.find((m) => m.id === form.modelId);
  const yilMin = secilenModel?.uretimBaslangicYili ?? 2020;
  const yilMax = secilenModel?.uretimBitisYili ?? new Date().getFullYear();

  useEffect(() => {
    if (secilenModel && (form.yil < yilMin || form.yil > yilMax)) {
      setForm((f) => ({ ...f, yil: Math.min(Math.max(f.yil, yilMin), yilMax), paketId: 0, motorId: 0 }));
    }
  }, [secilenModel, form.yil, yilMin, yilMax]);

  const { data: paketler } = useQuery({
    queryKey: ["paketler", form.modelId, form.yil],
    queryFn: () => api.paketler(form.modelId, form.yil),
    enabled: form.modelId > 0 && form.yil > 0,
  });

  const { data: motorlar } = useQuery({
    queryKey: ["motorlar", form.paketId],
    queryFn: () => api.motorSecenekleri(form.paketId),
    enabled: form.paketId > 0,
  });

  const olusturMutation = useMutation({
    mutationFn: async () => {
      if (!token) throw new Error("Oturum gerekli");
      return api.ilanOlustur(
        {
          kategori: form.kategori,
          motorSecenegiId: form.motorId,
          uretimYili: form.yil,
          kilometre: form.kilometre,
          fiyat: form.fiyat,
          renk: form.renk,
          vitesTipi: form.vitesTipi,
          aciklama: form.aciklama,
          hasarDurumu: form.hasarDurumu,
          gorselYollari: gorselYollari.length ? gorselYollari : undefined,
          expertizGorselYolu: expertizYolu ?? undefined,
        },
        token
      );
    },
    onSuccess: (data) => {
      router.push(`/ilanlar/${data.id}`);
    },
    onError: (err: Error) => {
      setHata(err.message);
    },
  });

  const gorselYukle = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !token) return;
    setHata(null);
    try {
      const yol = await api.gorselYukle(file, token);
      setGorselYollari((prev) => [...prev, yol]);
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Yükleme hatası");
    }
    e.target.value = "";
  };

  const expertizYukle = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !token) return;
    setHata(null);
    try {
      const yol = await api.expertizGorseliYukle(file, token);
      setExpertizYolu(yol);
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Yükleme hatası");
    }
    e.target.value = "";
  };

  if (!hydrated || !girisliMi) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <p className="text-slate-400">Yükleniyor...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-2xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-white">İlan Ver</h1>
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 shadow-lg backdrop-blur">
          <div className="mb-6 flex gap-2">
            {[1, 2, 3].map((a) => (
              <button
                key={a}
                onClick={() => setAdim(a)}
                className={`rounded px-4 py-2 text-sm font-medium ${
                  adim === a ? "bg-emerald-600 text-white" : "bg-slate-700/80 text-slate-400 hover:text-white"
                }`}
              >
                Adım {a}
              </button>
            ))}
          </div>

          {adim === 1 && (
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-slate-300">Kategori</label>
                <select
                  value={form.kategori}
                  onChange={(e) => setForm((f) => ({ ...f, kategori: Number(e.target.value), markaId: 0, modelId: 0, paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                >
                  {KATEGORILER.map((k) => (
                    <option key={k.deger} value={k.deger}>{k.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Marka</label>
                <select
                  value={form.markaId}
                  onChange={(e) => setForm((f) => ({ ...f, markaId: Number(e.target.value), modelId: 0, paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                >
                  <option value={0}>Seçin</option>
                  {markalar?.map((m) => (
                    <option key={m.id} value={m.id}>{m.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Model</label>
                <select
                  value={form.modelId}
                  onChange={(e) => setForm((f) => ({ ...f, modelId: Number(e.target.value), paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                  disabled={!modeller?.length}
                >
                  <option value={0}>Seçin</option>
                  {modeller?.map((m) => (
                    <option key={m.id} value={m.id}>{m.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Üretim Yılı</label>
                <input
                  type="number"
                  value={form.yil}
                  onChange={(e) => setForm((f) => ({ ...f, yil: Number(e.target.value), paketId: 0, motorId: 0 }))}
                  min={yilMin}
                  max={yilMax}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Paket</label>
                <select
                  value={form.paketId}
                  onChange={(e) => setForm((f) => ({ ...f, paketId: Number(e.target.value), motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                  disabled={!paketler?.length}
                >
                  <option value={0}>{paketler?.length ? "Seçin" : "Seçin (model ve yıl gerekli)"}</option>
                  {paketler?.map((p) => (
                    <option key={p.id} value={p.id}>{p.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Motor</label>
                <select
                  value={form.motorId}
                  onChange={(e) => setForm((f) => ({ ...f, motorId: Number(e.target.value) }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                  disabled={!motorlar?.length}
                >
                  <option value={0}>Seçin</option>
                  {motorlar?.map((m) => (
                    <option key={m.id} value={m.id}>{m.ad}</option>
                  ))}
                </select>
              </div>
              <button
                onClick={() => setAdim(2)}
                disabled={!form.motorId}
                className="mt-4 w-full rounded-lg bg-emerald-600 py-2 font-medium text-white hover:bg-emerald-500 disabled:opacity-50"
              >
                Devam
              </button>
            </div>
          )}

          {adim === 2 && (
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-slate-300">Kilometre</label>
                <input
                  type="number"
                  value={form.kilometre || ""}
                  onChange={(e) => setForm((f) => ({ ...f, kilometre: Number(e.target.value) || 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Fiyat (₺)</label>
                <input
                  type="number"
                  value={form.fiyat || ""}
                  onChange={(e) => setForm((f) => ({ ...f, fiyat: Number(e.target.value) || 0 }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Renk</label>
                <select
                  value={form.renk}
                  onChange={(e) => setForm((f) => ({ ...f, renk: e.target.value }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                >
                  <option value="">Seçin</option>
                  {RENKLER.map((r) => (
                    <option key={r} value={r}>{r}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Vites</label>
                <select
                  value={form.vitesTipi}
                  onChange={(e) => setForm((f) => ({ ...f, vitesTipi: e.target.value }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                >
                  <option>Manuel</option>
                  <option>Otomatik</option>
                  <option>Yarı Otomatik</option>
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Hasar Durumu</label>
                <select
                  value={form.hasarDurumu}
                  onChange={(e) => setForm((f) => ({ ...f, hasarDurumu: Number(e.target.value) }))}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                >
                  <option value={0}>Bilinmiyor</option>
                  <option value={1}>Hasarsız</option>
                  <option value={2}>Hasar Kayıtlı</option>
                  <option value={3}>Expertiz Var</option>
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Açıklama</label>
                <textarea
                  value={form.aciklama}
                  onChange={(e) => setForm((f) => ({ ...f, aciklama: e.target.value }))}
                  rows={4}
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white"
                />
              </div>
              <div className="flex gap-2">
                <button onClick={() => setAdim(1)} className="flex-1 rounded-lg border border-slate-600 bg-slate-700/80 py-2 text-white hover:bg-slate-600">
                  Geri
                </button>
                <button
                  onClick={() => setAdim(3)}
                  className="flex-1 rounded-lg bg-emerald-600 py-2 text-white hover:bg-emerald-500"
                >
                  Devam
                </button>
              </div>
            </div>
          )}

          {adim === 3 && (
            <div className="space-y-4">
              <div>
                <h3 className="font-medium text-white">Araç Fotoğrafları</h3>
                <input
                  ref={gorselInputRef}
                  type="file"
                  accept="image/*"
                  onChange={gorselYukle}
                  className="hidden"
                />
                <button
                  type="button"
                  onClick={() => gorselInputRef.current?.click()}
                  className="mt-2 rounded-lg border border-dashed border-slate-600 px-4 py-2 text-sm text-slate-400 hover:bg-slate-700/50"
                >
                  + Fotoğraf ekle
                </button>
                {gorselYollari.length > 0 && (
                  <p className="mt-1 text-sm text-slate-400">{gorselYollari.length} fotoğraf yüklendi</p>
                )}
              </div>
              {form.hasarDurumu === 3 && (
                <div>
                  <h3 className="font-medium text-white">Expertiz Görseli</h3>
                  <input
                    ref={expertizInputRef}
                    type="file"
                    accept="image/*"
                    onChange={expertizYukle}
                    className="hidden"
                  />
                  <button
                    type="button"
                    onClick={() => expertizInputRef.current?.click()}
                    className="mt-2 rounded-lg border border-dashed border-slate-600 px-4 py-2 text-sm text-slate-400 hover:bg-slate-700/50"
                  >
                    {expertizYolu ? "Expertiz yüklendi" : "+ Expertiz yükle"}
                  </button>
                </div>
              )}
              {hata && <p className="text-sm text-red-400">{hata}</p>}
              <div className="flex gap-2">
                <button onClick={() => setAdim(2)} className="flex-1 rounded-lg border border-slate-600 bg-slate-700/80 py-2 text-white hover:bg-slate-600">
                  Geri
                </button>
                <button
                  onClick={() => olusturMutation.mutate()}
                  disabled={olusturMutation.isPending}
                  className="flex-1 rounded-lg bg-emerald-600 py-2 font-medium text-white hover:bg-emerald-700 disabled:opacity-50"
                >
                  {olusturMutation.isPending ? "Gönderiliyor..." : "İlanı Oluştur"}
                </button>
              </div>
              <Link href="/" className="mt-4 inline-block text-sm text-slate-400 hover:text-white">
                ← Ana sayfaya dön
              </Link>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}
