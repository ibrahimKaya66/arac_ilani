"use client";

import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { IlanKarti } from "./IlanKarti";

export function IlanListe() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["ilanlar", { sayfa: 1, sayfaBoyutu: 12 }],
    queryFn: () => api.ilanlar({ sayfa: 1, sayfaBoyutu: 12 }),
  });

  if (isLoading) {
    return (
      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {[...Array(6)].map((_, i) => (
          <div key={i} className="h-80 animate-pulse rounded-lg bg-slate-200" />
        ))}
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-red-700">
        İlanlar yüklenemedi. API bağlantısını kontrol edin.
      </div>
    );
  }

  const ilanlar = data?.ilanlar ?? [];
  const toplam = data?.toplamKayit ?? 0;

  return (
    <section>
      <h2 className="mb-4 text-lg font-semibold text-slate-800">Son İlanlar ({toplam} ilan)</h2>
      {ilanlar.length === 0 ? (
        <div className="rounded-lg border border-slate-200 bg-white p-12 text-center text-slate-500">
          Henüz ilan bulunmuyor.
        </div>
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {ilanlar.map((ilan) => (
            <IlanKarti key={ilan.id} ilan={ilan} />
          ))}
        </div>
      )}
    </section>
  );
}
