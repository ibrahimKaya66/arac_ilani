"use client";

import Link from "next/link";
import { IlanKarti } from "./IlanKarti";
import type { IlanListe } from "@/lib/api";

interface BenimIlanKartiProps {
  ilan: IlanListe;
  onSil: (id: number) => void;
  siliniyor?: boolean;
}

export function BenimIlanKarti({ ilan, onSil, siliniyor }: BenimIlanKartiProps) {
  return (
    <div className="relative group">
      <Link href={`/ilanlar/${ilan.id}`} className="block">
        <IlanKarti ilan={ilan} dark />
      </Link>
      <button
        type="button"
        onClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          if (confirm("Bu ilanı silmek istediğinize emin misiniz?")) onSil(ilan.id);
        }}
        disabled={siliniyor}
        className="absolute right-2 top-2 z-10 rounded bg-red-600/90 px-3 py-1.5 text-sm font-medium text-white opacity-0 transition hover:bg-red-500 group-hover:opacity-100 disabled:opacity-50"
      >
        {siliniyor ? "..." : "Sil"}
      </button>
    </div>
  );
}
