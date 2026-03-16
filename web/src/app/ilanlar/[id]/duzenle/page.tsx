"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import Link from "next/link";
import { useMutation, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore, useAuthHydrated } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";

const RENKLER = [
  "Beyaz", "Siyah", "Gri", "Gümüş", "Kırmızı", "Mavi", "Yeşil", "Bej", "Kahverengi",
  "Turuncu", "Sarı", "Lacivert", "Bordo", "Antrasit", "Bronz", "Diğer",
];

export default function IlanDuzenlePage() {
  const params = useParams();
  const router = useRouter();
  const id = Number(params.id);
  const hydrated = useAuthHydrated();
  const { token, kullaniciId } = useAuthStore(useShallow((s) => ({ token: s.token, kullaniciId: s.kullaniciId })));
  const [form, setForm] = useState({ kilometre: 0, fiyat: 0, renk: "", vitesTipi: "Manuel", aciklama: "", hasarDurumu: 0 });
  const [hata, setHata] = useState<string | null>(null);

  const { data, isLoading, error } = useQuery({
    queryKey: ["ilan", id],
    queryFn: () => api.ilanDetay(id),
    enabled: !Number.isNaN(id) && !!token,
  });

  useEffect(() => {
    if (data) {
      setForm({
        kilometre: data.kilometre,
        fiyat: data.fiyat,
        renk: data.renk,
        vitesTipi: data.vitesTipi,
        aciklama: data.aciklama,
        hasarDurumu: data.hasarDurumu === "Bilinmiyor" ? 0 : data.hasarDurumu === "Hasarsiz" ? 1 : data.hasarDurumu === "HasarKayitli" ? 2 : data.hasarDurumu === "ExpertizVar" ? 3 : 0,
      });
    }
  }, [data]);

  const guncelleMutation = useMutation({
    mutationFn: () => api.ilanGuncelle(id, form, token!),
    onSuccess: () => router.push(`/ilanlar/${id}`),
    onError: (err: Error) => setHata(err.message),
  });

  if (!hydrated) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <p className="text-slate-400">Yükleniyor...</p>
      </div>
    );
  }

  if (!token) {
    router.push("/giris");
    return null;
  }

  if (isLoading || !data) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-2xl px-4 py-8">
          <div className="h-64 animate-pulse rounded-lg bg-slate-700" />
        </main>
      </div>
    );
  }

  if (error || (data.kullaniciId && data.kullaniciId !== kullaniciId)) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-2xl px-4 py-8">
          <p className="text-red-400">Bu ilanı düzenleyemezsiniz.</p>
          <Link href={`/ilanlar/${id}`} className="mt-4 inline-block text-slate-300 hover:text-white">← İlana dön</Link>
        </main>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-2xl px-4 py-8">
        <Link href={`/ilanlar/${id}`} className="mb-4 inline-block text-sm text-slate-300 hover:text-white">← İlana dön</Link>
        <h1 className="mb-6 text-2xl font-bold text-white">İlan Düzenle</h1>
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 shadow-lg backdrop-blur">
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
            {hata && <p className="text-sm text-red-400">{hata}</p>}
            <button
              onClick={() => guncelleMutation.mutate()}
              disabled={guncelleMutation.isPending}
              className="w-full rounded-lg bg-emerald-600 py-2 font-medium text-white hover:bg-emerald-500 disabled:opacity-50"
            >
              {guncelleMutation.isPending ? "Kaydediliyor..." : "Kaydet"}
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}
