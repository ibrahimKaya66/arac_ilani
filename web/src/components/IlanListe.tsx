"use client";

import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { IlanKarti } from "./IlanKarti";

export function IlanListe({ dark }: { dark?: boolean }) {
  const { data, isLoading, error } = useQuery({
    queryKey: ["ilanlar", { sayfa: 1, sayfaBoyutu: 12 }],
    queryFn: () => api.ilanlar({ sayfa: 1, sayfaBoyutu: 12 }),
  });

  const titleCls = dark ? "text-white" : "text-slate-800";

  if (isLoading) {
    return (
      <div className="space-y-4">
        <div className={`h-6 w-48 animate-pulse rounded ${dark ? "bg-slate-600" : "bg-slate-200"}`} />
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {[...Array(6)].map((_, i) => (
            <div key={i} className={`h-80 animate-pulse rounded-lg ${dark ? "bg-slate-700" : "bg-slate-200"}`} />
          ))}
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={`rounded-lg border p-4 ${dark ? "border-red-500/50 bg-red-900/20 text-red-300" : "border-red-200 bg-red-50 text-red-700"}`}>
        İlanlar yüklenemedi. API bağlantısını kontrol edin.
      </div>
    );
  }

  const ilanlar = data?.ilanlar ?? [];
  const toplam = data?.toplamKayit ?? 0;

  return (
    <section>
      <h2 className={`mb-6 text-xl font-bold sm:text-2xl ${titleCls}`}>Son İlanlar {toplam > 0 && `(${toplam} ilan)`}</h2>
      {ilanlar.length === 0 ? (
        <div className={`rounded-lg border p-12 text-center ${dark ? "border-slate-600 text-slate-400" : "border-slate-200 bg-white text-slate-500"}`}>
          Henüz ilan bulunmuyor.
        </div>
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {ilanlar.map((ilan) => (
            <IlanKarti key={ilan.id} ilan={ilan} dark={dark} />
          ))}
        </div>
      )}
    </section>
  );
}
