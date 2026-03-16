"use client";

import { useEffect, useState, useCallback } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore, useAuthHydrated } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";
import { BenimIlanKarti } from "@/components/BenimIlanKarti";
import { IlanFiltre, type IlanFiltreDegerleri } from "@/components/IlanFiltre";

export default function BenimIlanlarimPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const hydrated = useAuthHydrated();
  const { girisliMi, token } = useAuthStore(useShallow((s) => ({ girisliMi: s.girisliMi, token: s.token })));
  const [sayfa, setSayfa] = useState(1);
  const [filtre, setFiltre] = useState<IlanFiltreDegerleri>({});

  useEffect(() => {
    if (!hydrated) return;
    if (!girisliMi) router.push("/giris");
  }, [hydrated, girisliMi, router]);

  const queryParams = useCallback(() => {
    const p: Record<string, string | number> = { sayfa, sayfaBoyutu: 12 };
    if (filtre.kategori != null) p.kategori = filtre.kategori;
    if (filtre.markaId != null) p.markaId = filtre.markaId;
    if (filtre.modelId != null) p.modelId = filtre.modelId;
    if (filtre.minYil != null) p.minYil = filtre.minYil;
    if (filtre.maxYil != null) p.maxYil = filtre.maxYil;
    if (filtre.minFiyat != null) p.minFiyat = filtre.minFiyat;
    if (filtre.maxFiyat != null) p.maxFiyat = filtre.maxFiyat;
    if (filtre.minKm != null) p.minKm = filtre.minKm;
    if (filtre.maxKm != null) p.maxKm = filtre.maxKm;
    if (filtre.siralama) p.siralama = filtre.siralama;
    return p;
  }, [sayfa, filtre]);

  const { data, isLoading } = useQuery({
    queryKey: ["ilanlarim", queryParams(), token],
    queryFn: () => api.ilanlarim(queryParams(), token!),
    enabled: hydrated && girisliMi && !!token,
  });

  const silMutation = useMutation({
    mutationFn: (id: number) => api.ilanSil(id, token!),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ilanlarim"] });
    },
  });

  useEffect(() => {
    setSayfa(1);
  }, [filtre.kategori, filtre.markaId, filtre.modelId, filtre.minYil, filtre.maxYil, filtre.minFiyat, filtre.maxFiyat, filtre.minKm, filtre.maxKm, filtre.siralama]);

  if (!hydrated || !girisliMi) return null;

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-white">Benim İlanlarım</h1>
        <div className="mb-6">
          <IlanFiltre degerler={filtre} onChange={setFiltre} dark />
        </div>
        {isLoading ? (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {[1, 2, 3].map((i) => (
              <div key={i} className="h-64 animate-pulse rounded-lg bg-slate-700" />
            ))}
          </div>
        ) : data && data.ilanlar.length > 0 ? (
          <>
            <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
              {data.ilanlar.map((ilan) => (
                <BenimIlanKarti
                  key={ilan.id}
                  ilan={ilan}
                  onSil={(id) => silMutation.mutate(id)}
                  siliniyor={silMutation.isPending && silMutation.variables === ilan.id}
                />
              ))}
            </div>
            {data.toplamKayit > data.sayfaBoyutu && (
              <div className="mt-6 flex justify-center gap-2">
                <button
                  onClick={() => setSayfa((s) => Math.max(1, s - 1))}
                  disabled={sayfa <= 1}
                  className="rounded border border-slate-600 bg-slate-800 px-4 py-2 text-white hover:bg-slate-700 disabled:opacity-50"
                >
                  Önceki
                </button>
                <span className="flex items-center px-4 text-slate-300">
                  {sayfa} / {Math.ceil(data.toplamKayit / data.sayfaBoyutu)}
                </span>
                <button
                  onClick={() => setSayfa((s) => s + 1)}
                  disabled={sayfa * data.sayfaBoyutu >= data.toplamKayit}
                  className="rounded border border-slate-600 bg-slate-800 px-4 py-2 text-white hover:bg-slate-700 disabled:opacity-50"
                >
                  Sonraki
                </button>
              </div>
            )}
          </>
        ) : (
          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-12 text-center backdrop-blur">
            <p className="text-slate-400">Henüz ilanınız yok veya filtreye uygun ilan bulunamadı.</p>
            <Link href="/ilan-ver" className="mt-4 inline-block text-emerald-400 hover:text-emerald-300">
              İlan ver →
            </Link>
          </div>
        )}
      </main>
    </div>
  );
}
