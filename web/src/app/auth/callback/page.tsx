"use client";

import { Suspense, useEffect, useState } from "react";
import Link from "next/link";
import { useRouter, useSearchParams } from "next/navigation";
import { useAuthStore } from "@/lib/auth-store";

function AuthCallbackContent() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const girisYap = useAuthStore((s) => s.girisYap);
  const [hata, setHata] = useState<string | null>(null);

  useEffect(() => {
    const hataParam = searchParams.get("hata");
    if (hataParam) {
      setHata(decodeURIComponent(hataParam));
      return;
    }

    const token = searchParams.get("token");
    const kullaniciId = searchParams.get("kullaniciId");
    const ad = searchParams.get("ad") ?? "";
    const soyad = searchParams.get("soyad") ?? "";
    const email = searchParams.get("email") ?? "";
    const rollerStr = searchParams.get("roller") ?? "";
    const roller = rollerStr ? rollerStr.split(",") : [];

    if (token && kullaniciId) {
      girisYap(token, kullaniciId, ad, soyad, email, roller);
      router.replace("/");
      router.refresh();
    } else {
      router.replace("/giris");
    }
  }, [searchParams, girisYap, router]);

  if (hata) {
    return (
      <div className="flex min-h-screen flex-col items-center justify-center gap-4 bg-slate-900 p-4">
        <p className="max-w-md text-center text-red-400">{hata}</p>
        <Link href="/giris" className="text-emerald-400 hover:underline">
          Giriş sayfasına dön
        </Link>
      </div>
    );
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-900">
      <p className="text-slate-400">Giriş yapılıyor...</p>
    </div>
  );
}

export default function AuthCallbackPage() {
  return (
    <Suspense fallback={
      <div className="flex min-h-screen items-center justify-center bg-slate-900">
        <p className="text-slate-400">Yükleniyor...</p>
      </div>
    }>
      <AuthCallbackContent />
    </Suspense>
  );
}
