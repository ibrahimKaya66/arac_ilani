"use client";

import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";

const KATEGORILER = [
  { deger: 1, ad: "Otomobil" },
  { deger: 2, ad: "SUV" },
  { deger: 3, ad: "Pickup" },
];

export interface IlanFiltreDegerleri {
  kategori?: number;
  markaId?: number;
  modelId?: number;
  minYil?: number;
  maxYil?: number;
  minFiyat?: number;
  maxFiyat?: number;
  minKm?: number;
  maxKm?: number;
  siralama?: string;
}

interface IlanFiltreProps {
  degerler: IlanFiltreDegerleri;
  onChange: (v: IlanFiltreDegerleri) => void;
  dark?: boolean;
}

export function IlanFiltre({ degerler, onChange, dark }: IlanFiltreProps) {
  const { data: markalar } = useQuery({
    queryKey: ["markalar", degerler.kategori ?? 1],
    queryFn: () => api.markalar(degerler.kategori ?? 1),
    enabled: (degerler.kategori ?? 0) > 0,
  });

  const { data: modeller } = useQuery({
    queryKey: ["modeller", degerler.markaId],
    queryFn: () => api.modeller(degerler.markaId!),
    enabled: !!degerler.markaId,
  });

  const cls = dark ? "border-slate-600 bg-slate-800/80 text-white" : "border-slate-300 bg-white text-slate-900";

  return (
    <div className={`flex flex-wrap gap-3 rounded-lg border p-4 ${dark ? "border-slate-700/50" : "border-slate-200"}`}>
      <select
        value={degerler.kategori ?? ""}
        onChange={(e) => onChange({ ...degerler, kategori: e.target.value ? Number(e.target.value) : undefined, markaId: undefined, modelId: undefined })}
        className={`rounded border px-3 py-1.5 text-sm ${cls}`}
      >
        <option value="">Tüm kategoriler</option>
        {KATEGORILER.map((k) => (
          <option key={k.deger} value={k.deger}>{k.ad}</option>
        ))}
      </select>
      <select
        value={degerler.markaId ?? ""}
        onChange={(e) => onChange({ ...degerler, markaId: e.target.value ? Number(e.target.value) : undefined, modelId: undefined })}
        className={`rounded border px-3 py-1.5 text-sm ${cls}`}
        disabled={!markalar?.length}
      >
        <option value="">Marka</option>
        {markalar?.map((m) => (
          <option key={m.id} value={m.id}>{m.ad}</option>
        ))}
      </select>
      <select
        value={degerler.modelId ?? ""}
        onChange={(e) => onChange({ ...degerler, modelId: e.target.value ? Number(e.target.value) : undefined })}
        className={`rounded border px-3 py-1.5 text-sm ${cls}`}
        disabled={!modeller?.length}
      >
        <option value="">Model</option>
        {modeller?.map((m) => (
          <option key={m.id} value={m.id}>{m.ad}</option>
        ))}
      </select>
      <input
        type="number"
        placeholder="Min yıl"
        value={degerler.minYil ?? ""}
        onChange={(e) => onChange({ ...degerler, minYil: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-24 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <input
        type="number"
        placeholder="Max yıl"
        value={degerler.maxYil ?? ""}
        onChange={(e) => onChange({ ...degerler, maxYil: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-24 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <input
        type="number"
        placeholder="Min fiyat"
        value={degerler.minFiyat ?? ""}
        onChange={(e) => onChange({ ...degerler, minFiyat: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-28 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <input
        type="number"
        placeholder="Max fiyat"
        value={degerler.maxFiyat ?? ""}
        onChange={(e) => onChange({ ...degerler, maxFiyat: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-28 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <input
        type="number"
        placeholder="Min km"
        value={degerler.minKm ?? ""}
        onChange={(e) => onChange({ ...degerler, minKm: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-24 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <input
        type="number"
        placeholder="Max km"
        value={degerler.maxKm ?? ""}
        onChange={(e) => onChange({ ...degerler, maxKm: e.target.value ? Number(e.target.value) : undefined })}
        className={`w-24 rounded border px-2 py-1.5 text-sm ${cls}`}
      />
      <select
        value={degerler.siralama ?? ""}
        onChange={(e) => onChange({ ...degerler, siralama: e.target.value || undefined })}
        className={`rounded border px-3 py-1.5 text-sm ${cls}`}
      >
        <option value="">Tarih (yeni)</option>
        <option value="fiyat">Fiyat (artan)</option>
        <option value="fiyat_desc">Fiyat (azalan)</option>
        <option value="yil">Yıl (artan)</option>
        <option value="yil_desc">Yıl (azalan)</option>
        <option value="km">Km (artan)</option>
        <option value="km_desc">Km (azalan)</option>
      </select>
    </div>
  );
}
