"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useParams } from "next/navigation";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";

export default function IlanDetayPage() {
  const params = useParams();
  const id = Number(params.id);
  const queryClient = useQueryClient();
  const { token, kullaniciId } = useAuthStore(useShallow((s) => ({ token: s.token, kullaniciId: s.kullaniciId })));

  const banaAitTaslak = (d: { kullaniciId: string | null; ilanDurumu: string }) =>
    token && kullaniciId && d.kullaniciId === kullaniciId && d.ilanDurumu === "Taslak";

  const banaAitYayinda = (d: { kullaniciId: string | null; ilanDurumu: string }) =>
    token && kullaniciId && d.kullaniciId === kullaniciId && d.ilanDurumu === "Yayinda";

  const yayinlaMutation = useMutation({
    mutationFn: () => api.ilanYayinla(id, token!),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["ilan", id] }),
  });

  const satildiMutation = useMutation({
    mutationFn: () => api.ilanSatildi(id, token!),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["ilan", id] }),
  });

  const { data, isLoading, error } = useQuery({
    queryKey: ["ilan", id],
    queryFn: () => api.ilanDetay(id),
    enabled: !Number.isNaN(id),
  });

  if (isLoading || !data) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-4xl px-4 py-8">
          <div className="h-96 animate-pulse rounded-lg bg-slate-200" />
        </main>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-4xl px-4 py-8">
          <p className="text-red-400">İlan bulunamadı.</p>
          <Link href="/ilanlar" className="mt-4 inline-block text-slate-300 hover:text-white">
            ← İlanlara dön
          </Link>
        </main>
      </div>
    );
  }

  const gorselUrl = (path: string) =>
    `${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094"}${path}`;

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-4xl px-4 py-8">
        <Link href="/ilanlar" className="mb-4 inline-block text-sm text-slate-300 hover:text-white">
          ← İlanlara dön
        </Link>
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 shadow-lg backdrop-blur">
          <div className="grid gap-6 p-6 md:grid-cols-2">
            <div className="space-y-4">
              {data.gorselYollari?.length ? (
                <img
                  src={gorselUrl(data.gorselYollari[0])}
                  alt={data.baslik}
                  className="w-full rounded-lg object-cover"
                />
              ) : (
                <div className="flex aspect-video items-center justify-center rounded-lg bg-slate-800 text-slate-500">
                  Fotoğraf yok
                </div>
              )}
            </div>
            <div>
              {data.ilanDurumu === "Taslak" && (
                <span className="mb-2 inline-block rounded bg-amber-500/20 px-2 py-0.5 text-xs font-medium text-amber-300">
                  Taslak
                </span>
              )}
              {data.ilanDurumu === "Satildi" && (
                <span className="mb-2 inline-block rounded bg-slate-600 px-2 py-0.5 text-xs font-medium text-slate-300">
                  Satıldı
                </span>
              )}
              <h1 className="text-2xl font-bold text-white">{data.baslik}</h1>
              {banaAitTaslak(data) && (
                <button
                  onClick={() => yayinlaMutation.mutate()}
                  disabled={yayinlaMutation.isPending}
                  className="mt-2 rounded-lg bg-emerald-600 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-500 disabled:opacity-50"
                >
                  {yayinlaMutation.isPending ? "Yayınlanıyor..." : "İlanı Yayınla"}
                </button>
              )}
              {banaAitYayinda(data) && (
                <button
                  onClick={() => satildiMutation.mutate()}
                  disabled={satildiMutation.isPending}
                  className="mt-2 ml-2 rounded-lg border border-slate-500 bg-slate-700/80 px-4 py-2 text-sm font-medium text-white hover:bg-slate-600 disabled:opacity-50"
                >
                  {satildiMutation.isPending ? "İşleniyor..." : "Satıldı Olarak İşaretle"}
                </button>
              )}
              <p className="mt-2 text-3xl font-bold text-emerald-400">
                {data.fiyat.toLocaleString("tr-TR")} ₺
              </p>
              <dl className="mt-4 space-y-2 text-sm">
                <div className="flex justify-between">
                  <dt className="text-slate-400">Marka / Model</dt>
                  <dd className="text-slate-200">{data.markaAd} {data.modelAd}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-400">Yıl</dt>
                  <dd className="text-slate-200">{data.uretimYili}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-400">Kilometre</dt>
                  <dd className="text-slate-200">{data.kilometre.toLocaleString("tr-TR")} km</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-400">Renk</dt>
                  <dd className="text-slate-200">{data.renk}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-400">Vites</dt>
                  <dd className="text-slate-200">{data.vitesTipi}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-400">Hasar Durumu</dt>
                  <dd className="text-slate-200">{data.hasarDurumu}</dd>
                </div>
              </dl>
            </div>
          </div>
          <div className="border-t border-slate-700/50 p-6">
            <h2 className="font-semibold text-white">Açıklama</h2>
            <p className="mt-2 whitespace-pre-wrap text-slate-300">{data.aciklama}</p>
            {data.teknikOzelliklerJson && (
              <>
                <h2 className="mt-6 font-semibold text-white">Teknik Özellikler</h2>
                <pre className="mt-2 overflow-auto rounded bg-slate-900/50 p-4 text-sm text-slate-300">
                  {JSON.stringify(JSON.parse(data.teknikOzelliklerJson), null, 2)}
                </pre>
              </>
            )}
            {data.expertizAIAnalizi && (
              <>
                <h2 className="mt-6 font-semibold text-slate-800">Expertiz Analizi</h2>
                <pre className="mt-2 overflow-auto rounded bg-slate-50 p-4 text-sm">
                  {data.expertizAIAnalizi}
                </pre>
              </>
            )}
          </div>
        </div>
      </main>
    </div>
  );
}
