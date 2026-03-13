"use client";

import Link from "next/link";
import type { IlanListe } from "@/lib/api";

export function IlanKarti({ ilan, dark }: { ilan: IlanListe; dark?: boolean }) {
  const gorselUrl = ilan.kapakGorselYolu
    ? `${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094"}${ilan.kapakGorselYolu}`
    : null;

  const cardCls = dark
    ? "group block overflow-hidden rounded-lg border border-slate-700/50 bg-slate-800/80 shadow-lg backdrop-blur transition hover:border-slate-600"
    : "group block overflow-hidden rounded-lg border bg-white shadow-sm transition hover:shadow-md";

  return (
    <Link href={`/ilanlar/${ilan.id}`} className={cardCls}>
      <div className={`relative aspect-[4/3] ${dark ? "bg-slate-800" : "bg-slate-100"}`}>
        {gorselUrl ? (
          <img
            src={gorselUrl}
            alt={ilan.baslik}
            className="h-full w-full object-cover transition group-hover:scale-105"
          />
        ) : (
          <div className={`flex h-full items-center justify-center ${dark ? "text-slate-500" : "text-slate-400"}`}>Fotoğraf yok</div>
        )}
      </div>
      <div className="p-4">
        <h3 className={`font-semibold line-clamp-2 ${dark ? "text-white" : "text-slate-900"}`}>{ilan.baslik}</h3>
        <p className={`mt-1 text-sm ${dark ? "text-slate-400" : "text-slate-600"}`}>
          {ilan.markaAd} {ilan.modelAd}
        </p>
        <div className="mt-2 flex items-center justify-between text-sm">
          <span className={`font-bold ${dark ? "text-emerald-400" : "text-emerald-600"}`}>{ilan.fiyat.toLocaleString("tr-TR")} ₺</span>
          <span className={dark ? "text-slate-400" : "text-slate-500"}>{ilan.kilometre.toLocaleString("tr-TR")} km</span>
        </div>
        <p className={`mt-1 text-xs ${dark ? "text-slate-500" : "text-slate-500"}`}>Hasar: {ilan.hasarDurumu}</p>
      </div>
    </Link>
  );
}
