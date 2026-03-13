"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";
import { IlanKarti } from "@/components/IlanKarti";

export default function BenimIlanlarimPage() {
  const router = useRouter();
  const { girisliMi, token } = useAuthStore(useShallow((s) => ({ girisliMi: s.girisliMi, token: s.token })));
  const [sayfa, setSayfa] = useState(1);

  useEffect(() => {
    if (!girisliMi) router.push("/giris");
  }, [girisliMi, router]);

  const { data, isLoading } = useQuery({
    queryKey: ["ilanlarim", sayfa, token],
    queryFn: () => api.ilanlarim(sayfa, 12, token!),
    enabled: girisliMi && !!token,
  });

  if (!girisliMi) return null;

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-white">Benim İlanlarım</h1>
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
                <IlanKarti key={ilan.id} ilan={ilan} dark />
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
            <p className="text-slate-400">Henüz ilanınız yok.</p>
            <Link href="/ilan-ver" className="mt-4 inline-block text-emerald-400 hover:text-emerald-300">
              İlan ver →
            </Link>
          </div>
        )}
      </main>
    </div>
  );
}
