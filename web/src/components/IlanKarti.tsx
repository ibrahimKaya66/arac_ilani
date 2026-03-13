"use client";

import Link from "next/link";
import type { IlanListe } from "@/lib/api";

export function IlanKarti({ ilan }: { ilan: IlanListe }) {
  const gorselUrl = ilan.kapakGorselYolu
    ? `${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000"}${ilan.kapakGorselYolu}`
    : null;

  return (
    <Link href={`/ilanlar/${ilan.id}`} className="group block overflow-hidden rounded-lg border bg-white shadow-sm transition hover:shadow-md">
      <div className="relative aspect-[4/3] bg-slate-100">
        {gorselUrl ? (
          <img
            src={gorselUrl}
            alt={ilan.baslik}
            className="h-full w-full object-cover transition group-hover:scale-105"
          />
        ) : (
          <div className="flex h-full items-center justify-center text-slate-400">Fotoğraf yok</div>
        )}
      </div>
      <div className="p-4">
        <h3 className="font-semibold text-slate-900 line-clamp-2">{ilan.baslik}</h3>
        <p className="mt-1 text-sm text-slate-600">
          {ilan.markaAd} {ilan.modelAd}
        </p>
        <div className="mt-2 flex items-center justify-between text-sm">
          <span className="font-bold text-emerald-600">{ilan.fiyat.toLocaleString("tr-TR")} ₺</span>
          <span className="text-slate-500">{ilan.kilometre.toLocaleString("tr-TR")} km</span>
        </div>
        <p className="mt-1 text-xs text-slate-500">Hasar: {ilan.hasarDurumu}</p>
      </div>
    </Link>
  );
}
