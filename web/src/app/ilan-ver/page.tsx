"use client";

import { useEffect, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useMutation, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";

const KATEGORILER = [
  { deger: 1, ad: "Otomobil" },
  { deger: 2, ad: "SUV" },
  { deger: 3, ad: "Pickup" },
];

export default function IlanVerPage() {
  const router = useRouter();
  const { girisliMi, token } = useAuthStore((s) => ({ girisliMi: s.girisliMi, token: s.token }));
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
    if (!girisliMi) router.push("/giris");
  }, [girisliMi, router]);

  const { data: markalar } = useQuery({
    queryKey: ["markalar", form.kategori],
    queryFn: () => api.markalar(form.kategori),
  });

  const { data: modeller } = useQuery({
    queryKey: ["modeller", form.markaId],
    queryFn: () => api.modeller(form.markaId),
    enabled: form.markaId > 0,
  });

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

  if (!girisliMi) return null;

  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-2xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-slate-800">İlan Ver</h1>
        <div className="rounded-xl border bg-white p-6 shadow-sm">
          <div className="mb-6 flex gap-2">
            {[1, 2, 3].map((a) => (
              <button
                key={a}
                onClick={() => setAdim(a)}
                className={`rounded px-4 py-2 text-sm font-medium ${
                  adim === a ? "bg-slate-900 text-white" : "bg-slate-100 text-slate-600"
                }`}
              >
                Adım {a}
              </button>
            ))}
          </div>

          {adim === 1 && (
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium">Kategori</label>
                <select
                  value={form.kategori}
                  onChange={(e) => setForm((f) => ({ ...f, kategori: Number(e.target.value), markaId: 0, modelId: 0, paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                >
                  {KATEGORILER.map((k) => (
                    <option key={k.deger} value={k.deger}>{k.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Marka</label>
                <select
                  value={form.markaId}
                  onChange={(e) => setForm((f) => ({ ...f, markaId: Number(e.target.value), modelId: 0, paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                >
                  <option value={0}>Seçin</option>
                  {markalar?.map((m) => (
                    <option key={m.id} value={m.id}>{m.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Model</label>
                <select
                  value={form.modelId}
                  onChange={(e) => setForm((f) => ({ ...f, modelId: Number(e.target.value), paketId: 0, motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                  disabled={!modeller?.length}
                >
                  <option value={0}>Seçin</option>
                  {modeller?.map((m) => (
                    <option key={m.id} value={m.id}>{m.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Üretim Yılı</label>
                <input
                  type="number"
                  value={form.yil}
                  onChange={(e) => setForm((f) => ({ ...f, yil: Number(e.target.value), paketId: 0, motorId: 0 }))}
                  min={1990}
                  max={new Date().getFullYear() + 1}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Paket</label>
                <select
                  value={form.paketId}
                  onChange={(e) => setForm((f) => ({ ...f, paketId: Number(e.target.value), motorId: 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                  disabled={!paketler?.length}
                >
                  <option value={0}>Seçin</option>
                  {paketler?.map((p) => (
                    <option key={p.id} value={p.id}>{p.ad}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Motor</label>
                <select
                  value={form.motorId}
                  onChange={(e) => setForm((f) => ({ ...f, motorId: Number(e.target.value) }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
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
                className="mt-4 w-full rounded-lg bg-slate-900 py-2 font-medium text-white disabled:opacity-50"
              >
                Devam
              </button>
            </div>
          )}

          {adim === 2 && (
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium">Kilometre</label>
                <input
                  type="number"
                  value={form.kilometre || ""}
                  onChange={(e) => setForm((f) => ({ ...f, kilometre: Number(e.target.value) || 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Fiyat (₺)</label>
                <input
                  type="number"
                  value={form.fiyat || ""}
                  onChange={(e) => setForm((f) => ({ ...f, fiyat: Number(e.target.value) || 0 }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Renk</label>
                <input
                  type="text"
                  value={form.renk}
                  onChange={(e) => setForm((f) => ({ ...f, renk: e.target.value }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Vites</label>
                <select
                  value={form.vitesTipi}
                  onChange={(e) => setForm((f) => ({ ...f, vitesTipi: e.target.value }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                >
                  <option>Manuel</option>
                  <option>Otomatik</option>
                  <option>Yarı Otomatik</option>
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Hasar Durumu</label>
                <select
                  value={form.hasarDurumu}
                  onChange={(e) => setForm((f) => ({ ...f, hasarDurumu: Number(e.target.value) }))}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                >
                  <option value={0}>Bilinmiyor</option>
                  <option value={1}>Hasarsız</option>
                  <option value={2}>Hasar Kayıtlı</option>
                  <option value={3}>Expertiz Var</option>
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium">Açıklama</label>
                <textarea
                  value={form.aciklama}
                  onChange={(e) => setForm((f) => ({ ...f, aciklama: e.target.value }))}
                  rows={4}
                  className="mt-1 w-full rounded-lg border px-3 py-2"
                />
              </div>
              <div className="flex gap-2">
                <button onClick={() => setAdim(1)} className="flex-1 rounded-lg border py-2">
                  Geri
                </button>
                <button
                  onClick={() => setAdim(3)}
                  className="flex-1 rounded-lg bg-slate-900 py-2 text-white"
                >
                  Devam
                </button>
              </div>
            </div>
          )}

          {adim === 3 && (
            <div className="space-y-4">
              <div>
                <h3 className="font-medium text-slate-800">Araç Fotoğrafları</h3>
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
                  className="mt-2 rounded-lg border border-dashed px-4 py-2 text-sm text-slate-600 hover:bg-slate-50"
                >
                  + Fotoğraf ekle
                </button>
                {gorselYollari.length > 0 && (
                  <p className="mt-1 text-sm text-slate-500">{gorselYollari.length} fotoğraf yüklendi</p>
                )}
              </div>
              {form.hasarDurumu === 3 && (
                <div>
                  <h3 className="font-medium text-slate-800">Expertiz Görseli</h3>
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
                    className="mt-2 rounded-lg border border-dashed px-4 py-2 text-sm text-slate-600 hover:bg-slate-50"
                  >
                    {expertizYolu ? "Expertiz yüklendi" : "+ Expertiz yükle"}
                  </button>
                </div>
              )}
              {hata && <p className="text-sm text-red-600">{hata}</p>}
              <div className="flex gap-2">
                <button onClick={() => setAdim(2)} className="flex-1 rounded-lg border py-2">
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
              <Link href="/" className="mt-4 inline-block text-sm text-slate-600 hover:underline">
                ← Ana sayfaya dön
              </Link>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}
