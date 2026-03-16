"use client";

import { useState, useCallback, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { IlanKarti } from "@/components/IlanKarti";
import { IlanFiltre, type IlanFiltreDegerleri } from "@/components/IlanFiltre";

export function IlanlarIcerik() {
  const [sayfa, setSayfa] = useState(1);
  const [filtre, setFiltre] = useState<IlanFiltreDegerleri>({});

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

  useEffect(() => {
    setSayfa(1);
  }, [filtre.kategori, filtre.markaId, filtre.modelId, filtre.minYil, filtre.maxYil, filtre.minFiyat, filtre.maxFiyat, filtre.minKm, filtre.maxKm, filtre.siralama]);

  const { data, isLoading, error } = useQuery({
    queryKey: ["ilanlar", queryParams()],
    queryFn: () => api.ilanlar(queryParams()),
  });

  const ilanlar = data?.ilanlar ?? [];
  const toplam = data?.toplamKayit ?? 0;
  const sayfaBoyutu = data?.sayfaBoyutu ?? 12;

  return (
    <>
      <div className="mb-6">
        <IlanFiltre degerler={filtre} onChange={setFiltre} dark />
      </div>
      {isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <div key={i} className="h-80 animate-pulse rounded-lg bg-slate-700" />
          ))}
        </div>
      ) : error ? (
        <div className="rounded-lg border border-red-500/50 bg-red-900/20 p-4 text-red-300">
          İlanlar yüklenemedi. API bağlantısını kontrol edin.
        </div>
      ) : ilanlar.length > 0 ? (
        <>
          <h2 className="mb-6 text-xl font-bold text-white sm:text-2xl">
            Tüm İlanlar {toplam > 0 && `(${toplam} ilan)`}
          </h2>
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {ilanlar.map((ilan) => (
              <IlanKarti key={ilan.id} ilan={ilan} dark />
            ))}
          </div>
          {toplam > sayfaBoyutu && (
            <div className="mt-6 flex justify-center gap-2">
              <button
                onClick={() => setSayfa((s) => Math.max(1, s - 1))}
                disabled={sayfa <= 1}
                className="rounded border border-slate-600 bg-slate-800 px-4 py-2 text-white hover:bg-slate-700 disabled:opacity-50"
              >
                Önceki
              </button>
              <span className="flex items-center px-4 text-slate-300">
                {sayfa} / {Math.ceil(toplam / sayfaBoyutu)}
              </span>
              <button
                onClick={() => setSayfa((s) => s + 1)}
                disabled={sayfa * sayfaBoyutu >= toplam}
                className="rounded border border-slate-600 bg-slate-800 px-4 py-2 text-white hover:bg-slate-700 disabled:opacity-50"
              >
                Sonraki
              </button>
            </div>
          )}
        </>
      ) : (
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-12 text-center backdrop-blur">
          <p className="text-slate-400">Henüz ilan bulunmuyor veya filtreye uygun ilan yok.</p>
        </div>
      )}
    </>
  );
}
