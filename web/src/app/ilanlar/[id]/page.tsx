"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useParams } from "next/navigation";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";

export default function IlanDetayPage() {
  const params = useParams();
  const id = Number(params.id);
  const queryClient = useQueryClient();
  const { token, kullaniciId } = useAuthStore((s) => ({ token: s.token, kullaniciId: s.kullaniciId }));

  const banaAitTaslak = (d: { kullaniciId: string | null; ilanDurumu: string }) =>
    token && kullaniciId && d.kullaniciId === kullaniciId && d.ilanDurumu === "Taslak";

  const yayinlaMutation = useMutation({
    mutationFn: () => api.ilanYayinla(id, token!),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["ilan", id] }),
  });

  const { data, isLoading, error } = useQuery({
    queryKey: ["ilan", id],
    queryFn: () => api.ilanDetay(id),
    enabled: !Number.isNaN(id),
  });

  if (isLoading || !data) {
    return (
      <div className="min-h-screen bg-slate-50">
        <Header />
        <main className="mx-auto max-w-4xl px-4 py-8">
          <div className="h-96 animate-pulse rounded-lg bg-slate-200" />
        </main>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-slate-50">
        <Header />
        <main className="mx-auto max-w-4xl px-4 py-8">
          <p className="text-red-600">İlan bulunamadı.</p>
          <Link href="/ilanlar" className="mt-4 inline-block text-slate-600 hover:underline">
            ← İlanlara dön
          </Link>
        </main>
      </div>
    );
  }

  const gorselUrl = (path: string) =>
    `${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000"}${path}`;

  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-4xl px-4 py-8">
        <Link href="/ilanlar" className="mb-4 inline-block text-sm text-slate-600 hover:underline">
          ← İlanlara dön
        </Link>
        <div className="rounded-xl border bg-white shadow-sm">
          <div className="grid gap-6 p-6 md:grid-cols-2">
            <div className="space-y-4">
              {data.gorselYollari?.length ? (
                <img
                  src={gorselUrl(data.gorselYollari[0])}
                  alt={data.baslik}
                  className="w-full rounded-lg object-cover"
                />
              ) : (
                <div className="flex aspect-video items-center justify-center rounded-lg bg-slate-100 text-slate-400">
                  Fotoğraf yok
                </div>
              )}
            </div>
            <div>
              {data.ilanDurumu === "Taslak" && (
                <span className="mb-2 inline-block rounded bg-amber-100 px-2 py-0.5 text-xs font-medium text-amber-800">
                  Taslak
                </span>
              )}
              <h1 className="text-2xl font-bold text-slate-900">{data.baslik}</h1>
              {banaAitTaslak(data) && (
                <button
                  onClick={() => yayinlaMutation.mutate()}
                  disabled={yayinlaMutation.isPending}
                  className="mt-2 rounded-lg bg-emerald-600 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-700 disabled:opacity-50"
                >
                  {yayinlaMutation.isPending ? "Yayınlanıyor..." : "İlanı Yayınla"}
                </button>
              )}
              <p className="mt-2 text-3xl font-bold text-emerald-600">
                {data.fiyat.toLocaleString("tr-TR")} ₺
              </p>
              <dl className="mt-4 space-y-2 text-sm">
                <div className="flex justify-between">
                  <dt className="text-slate-500">Marka / Model</dt>
                  <dd>{data.markaAd} {data.modelAd}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-500">Yıl</dt>
                  <dd>{data.uretimYili}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-500">Kilometre</dt>
                  <dd>{data.kilometre.toLocaleString("tr-TR")} km</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-500">Renk</dt>
                  <dd>{data.renk}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-500">Vites</dt>
                  <dd>{data.vitesTipi}</dd>
                </div>
                <div className="flex justify-between">
                  <dt className="text-slate-500">Hasar Durumu</dt>
                  <dd>{data.hasarDurumu}</dd>
                </div>
              </dl>
            </div>
          </div>
          <div className="border-t p-6">
            <h2 className="font-semibold text-slate-800">Açıklama</h2>
            <p className="mt-2 whitespace-pre-wrap text-slate-600">{data.aciklama}</p>
            {data.teknikOzelliklerJson && (
              <>
                <h2 className="mt-6 font-semibold text-slate-800">Teknik Özellikler</h2>
                <pre className="mt-2 overflow-auto rounded bg-slate-50 p-4 text-sm">
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
