"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";

export default function GirisPage() {
  const router = useRouter();
  const girisYap = useAuthStore((s) => s.girisYap);
  const [email, setEmail] = useState("");
  const [sifre, setSifre] = useState("");
  const [hata, setHata] = useState("");
  const [yukleniyor, setYukleniyor] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setHata("");
    setYukleniyor(true);
    try {
      const sonuc = await api.kimlik.giris({ email, sifre });
      girisYap(sonuc.token, sonuc.kullaniciId, sonuc.ad, sonuc.soyad, sonuc.email, sonuc.roller);
      router.push("/");
      router.refresh();
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Giriş başarısız");
    } finally {
      setYukleniyor(false);
    }
  };

  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-md px-4 py-12">
        <div className="rounded-xl border bg-white p-8 shadow-sm">
          <h1 className="text-xl font-bold text-slate-800">Giriş Yap</h1>
          <form onSubmit={handleSubmit} className="mt-6 space-y-4">
            <div>
              <label className="block text-sm font-medium text-slate-700">E-posta</label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700">Şifre</label>
              <input
                type="password"
                value={sifre}
                onChange={(e) => setSifre(e.target.value)}
                required
                className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
              />
            </div>
            {hata && <p className="text-sm text-red-600">{hata}</p>}
            <button
              type="submit"
              disabled={yukleniyor}
              className="w-full rounded-lg bg-slate-900 py-2 font-medium text-white hover:bg-slate-800 disabled:opacity-50"
            >
              {yukleniyor ? "Giriş yapılıyor..." : "Giriş Yap"}
            </button>
          </form>
          <p className="mt-4 text-center text-sm text-slate-600">
            Hesabınız yok mu?{" "}
            <Link href="/kayit" className="font-medium text-slate-900 hover:underline">
              Kayıt olun
            </Link>
          </p>
        </div>
      </main>
    </div>
  );
}
