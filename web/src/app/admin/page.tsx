"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore, useAuthHydrated } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";

function formatTarih(d: Date) {
  return d.toISOString().slice(0, 10);
}

export default function AdminPage() {
  const router = useRouter();
  const hydrated = useAuthHydrated();
  const { girisliMi, token, roller } = useAuthStore(useShallow((s) => ({ girisliMi: s.girisliMi, token: s.token, roller: s.roller })));
  const adminMi = roller?.includes("Admin") ?? false;

  const [baslangic, setBaslangic] = useState(formatTarih(new Date(Date.now() - 90 * 24 * 60 * 60 * 1000)));
  const [bitis, setBitis] = useState(formatTarih(new Date()));

  useEffect(() => {
    if (!hydrated) return;
    if (!girisliMi) router.push("/giris");
    else if (!adminMi) router.push("/");
  }, [hydrated, girisliMi, adminMi, router]);

  const { data: tarihRaporu } = useQuery({
    queryKey: ["rapor-tarih", baslangic, bitis, token],
    queryFn: () => api.raporlar.tarihAraligi(baslangic, bitis, token!),
    enabled: hydrated && girisliMi && !!token && adminMi,
  });

  const { data: yilRaporu } = useQuery({
    queryKey: ["rapor-yil", baslangic, bitis, token],
    queryFn: () => api.raporlar.uretimYiliSatis(baslangic, bitis, token!),
    enabled: hydrated && girisliMi && !!token && adminMi,
  });

  const { data: hizliSatilanlar } = useQuery({
    queryKey: ["rapor-hizli", token],
    queryFn: () => api.raporlar.enHizliSatilanlar(10, token!),
    enabled: hydrated && girisliMi && !!token && adminMi,
  });

  if (!hydrated || !girisliMi || !adminMi) return null;

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-8 text-2xl font-bold text-white">Admin Dashboard</h1>

        <div className="mb-8 flex flex-wrap gap-4">
          <div>
            <label className="block text-xs text-slate-400">Başlangıç</label>
            <input
              type="date"
              value={baslangic}
              onChange={(e) => setBaslangic(e.target.value)}
              className="mt-1 rounded-lg border border-slate-600 bg-slate-800 px-3 py-2 text-white"
            />
          </div>
          <div>
            <label className="block text-xs text-slate-400">Bitiş</label>
            <input
              type="date"
              value={bitis}
              onChange={(e) => setBitis(e.target.value)}
              className="mt-1 rounded-lg border border-slate-600 bg-slate-800 px-3 py-2 text-white"
            />
          </div>
        </div>

        <div className="grid gap-6 md:grid-cols-3">
          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 backdrop-blur">
            <h2 className="text-sm font-medium text-slate-400">Toplam Satış</h2>
            <p className="mt-2 text-3xl font-bold text-emerald-400">{tarihRaporu?.toplamSatis ?? 0}</p>
          </div>
          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 backdrop-blur">
            <h2 className="text-sm font-medium text-slate-400">Toplam Ciro (₺)</h2>
            <p className="mt-2 text-3xl font-bold text-emerald-400">
              {(tarihRaporu?.toplamCiro ?? 0).toLocaleString("tr-TR")}
            </p>
          </div>
          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 backdrop-blur">
            <h2 className="text-sm font-medium text-slate-400">Aktif İlan</h2>
            <p className="mt-2 text-3xl font-bold text-sky-400">{tarihRaporu?.aktifIlanSayisi ?? 0}</p>
          </div>
        </div>

        <div className="mt-8 grid gap-6 lg:grid-cols-2">
          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 backdrop-blur">
            <h2 className="mb-4 font-semibold text-white">Üretim Yılına Göre Satış</h2>
            {yilRaporu && yilRaporu.length > 0 ? (
              <div className="space-y-2">
                {yilRaporu.map((r) => (
                  <div key={r.uretimYili} className="flex justify-between rounded-lg bg-slate-900/50 px-4 py-2">
                    <span className="text-slate-300">{r.uretimYili} model</span>
                    <span className="text-emerald-400">{r.satisSayisi} satış</span>
                    <span className="text-slate-400">{r.toplamCiro.toLocaleString("tr-TR")} ₺</span>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-slate-500">Bu tarih aralığında satış verisi yok.</p>
            )}
          </div>

          <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 backdrop-blur">
            <h2 className="mb-4 font-semibold text-white">En Hızlı Satılanlar</h2>
            {hizliSatilanlar && hizliSatilanlar.length > 0 ? (
              <div className="space-y-2">
                {hizliSatilanlar.map((r) => (
                  <div key={r.ilanId} className="flex justify-between rounded-lg bg-slate-900/50 px-4 py-2">
                    <span className="truncate text-slate-300" title={r.baslik}>{r.baslik}</span>
                    <span className="shrink-0 text-emerald-400">{r.satisSuresiGun} gün</span>
                    <span className="shrink-0 text-slate-400">{r.fiyat.toLocaleString("tr-TR")} ₺</span>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-slate-500">Henüz satılan ilan yok.</p>
            )}
          </div>
        </div>
      </main>
    </div>
  );
}
