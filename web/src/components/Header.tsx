"use client";

import Link from "next/link";
import { useAuthStore } from "@/lib/auth-store";

export function Header() {
  const { girisliMi, ad, cikisYap } = useAuthStore();

  return (
    <header className="sticky top-0 z-50 border-b bg-white/95 backdrop-blur">
      <div className="mx-auto flex h-14 max-w-6xl items-center justify-between px-4">
        <Link href="/" className="text-xl font-bold text-slate-800">
          Araç İlan
        </Link>
        <nav className="flex items-center gap-6">
          <Link href="/ilanlar" className="text-sm font-medium text-slate-600 hover:text-slate-900">
            İlanlar
          </Link>
          {girisliMi ? (
            <>
              <Link href="/benim-ilanlarim" className="text-sm font-medium text-slate-600 hover:text-slate-900">
                Benim İlanlarım
              </Link>
              <Link href="/ilan-ver" className="rounded-full bg-emerald-600 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-700">
                İlan Ver
              </Link>
              <span className="text-sm text-slate-600">{ad}</span>
              <button onClick={cikisYap} className="text-sm text-slate-500 hover:text-slate-900">
                Çıkış
              </button>
            </>
          ) : (
            <>
              <Link href="/giris" className="text-sm font-medium text-slate-600 hover:text-slate-900">
                Giriş
              </Link>
              <Link href="/kayit" className="rounded-full bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800">
                Kayıt Ol
              </Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
}
