"use client";

import Link from "next/link";
import { useAuthStore } from "@/lib/auth-store";

export function HeroCta() {
  const girisliMi = useAuthStore((s) => s.girisliMi);

  return (
    <section className="relative overflow-hidden rounded-2xl border border-slate-700/50 bg-gradient-to-br from-emerald-900/40 via-slate-800/60 to-slate-900/80 p-8 backdrop-blur sm:p-12 md:p-16">
      <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_top_right,_var(--tw-gradient-stops))] from-emerald-500/10 via-transparent to-transparent" />
      <div className="relative">
        <h1 className="text-3xl font-bold tracking-tight text-white sm:text-4xl md:text-5xl">
          Araç Alım Satım
          <span className="block bg-gradient-to-r from-emerald-400 to-teal-400 bg-clip-text text-transparent">
            Platformu
          </span>
        </h1>
        <p className="mt-4 max-w-xl text-lg text-slate-300">
          Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım.
        </p>
        {!girisliMi && (
          <div className="mt-8 flex flex-wrap gap-4">
            <Link
              href="/kayit"
              className="inline-flex items-center rounded-xl bg-emerald-500 px-6 py-3 font-semibold text-white shadow-lg shadow-emerald-500/25 transition hover:bg-emerald-400 hover:shadow-emerald-500/30"
            >
              Kayıt Ol
            </Link>
            <Link
              href="/giris"
              className="inline-flex items-center rounded-xl border border-slate-500 bg-slate-800/80 px-6 py-3 font-semibold text-white backdrop-blur transition hover:border-slate-400 hover:bg-slate-700/80"
            >
              Giriş Yap
            </Link>
          </div>
        )}
      </div>
    </section>
  );
}
