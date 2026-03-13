"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";
import { IlanKarti } from "@/components/IlanKarti";

export default function BenimIlanlarimPage() {
  const router = useRouter();
  const { girisliMi, token } = useAuthStore((s) => ({ girisliMi: s.girisliMi, token: s.token }));
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
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-slate-800">Benim İlanlarım</h1>
        {isLoading ? (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {[1, 2, 3].map((i) => (
              <div key={i} className="h-64 animate-pulse rounded-lg bg-slate-200" />
            ))}
          </div>
        ) : data && data.ilanlar.length > 0 ? (
          <>
            <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
              {data.ilanlar.map((ilan) => (
                <IlanKarti key={ilan.id} ilan={ilan} />
              ))}
            </div>
            {data.toplamKayit > data.sayfaBoyutu && (
              <div className="mt-6 flex justify-center gap-2">
                <button
                  onClick={() => setSayfa((s) => Math.max(1, s - 1))}
                  disabled={sayfa <= 1}
                  className="rounded border px-4 py-2 disabled:opacity-50"
                >
                  Önceki
                </button>
                <span className="flex items-center px-4">
                  {sayfa} / {Math.ceil(data.toplamKayit / data.sayfaBoyutu)}
                </span>
                <button
                  onClick={() => setSayfa((s) => s + 1)}
                  disabled={sayfa * data.sayfaBoyutu >= data.toplamKayit}
                  className="rounded border px-4 py-2 disabled:opacity-50"
                >
                  Sonraki
                </button>
              </div>
            )}
          </>
        ) : (
          <div className="rounded-xl border bg-white p-12 text-center">
            <p className="text-slate-600">Henüz ilanınız yok.</p>
            <Link href="/ilan-ver" className="mt-4 inline-block text-emerald-600 hover:underline">
              İlan ver →
            </Link>
          </div>
        )}
      </main>
    </div>
  );
}
