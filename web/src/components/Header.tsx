"use client";

import Link from "next/link";
import { useAuthStore } from "@/lib/auth-store";

export function Header() {
  const { girisliMi, ad, roller, cikisYap } = useAuthStore();
  const adminMi = roller?.includes("Admin") ?? false;

  return (
    <header className="sticky top-0 z-50 border-b border-slate-700/50 bg-slate-900/95 backdrop-blur">
      <div className="mx-auto flex h-14 max-w-6xl items-center justify-between px-4">
        <Link href="/" className="text-xl font-bold text-white">
          Araç İlan
        </Link>
        <nav className="flex items-center gap-6">
          <Link href="/ilanlar" className="text-sm font-medium text-slate-300 hover:text-white">
            İlanlar
          </Link>
          {girisliMi ? (
            <>
              {adminMi && (
                <Link href="/admin" className="text-sm font-medium text-slate-300 hover:text-white">
                  Admin
                </Link>
              )}
              <Link href="/benim-ilanlarim" className="text-sm font-medium text-slate-300 hover:text-white">
                Benim İlanlarım
              </Link>
              <Link href="/ilan-ver" className="rounded-full bg-emerald-500 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-400">
                İlan Ver
              </Link>
              <span className="text-sm text-slate-400">{ad}</span>
              <button onClick={cikisYap} className="text-sm text-slate-400 hover:text-white">
                Çıkış
              </button>
            </>
          ) : (
            <>
              <Link href="/giris" className="text-sm font-medium text-slate-300 hover:text-white">
                Giriş Yap
              </Link>
              <Link href="/kayit" className="rounded-full bg-emerald-500 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-400">
                Kayıt Ol
              </Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
}
