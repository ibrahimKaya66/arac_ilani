"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";

export default function KayitPage() {
  const router = useRouter();
  const girisYap = useAuthStore((s) => s.girisYap);
  const [form, setForm] = useState({ email: "", sifre: "", ad: "", soyad: "", telefon: "" });
  const [hata, setHata] = useState("");
  const [yukleniyor, setYukleniyor] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setHata("");
    setYukleniyor(true);
    try {
      const sonuc = await api.kimlik.kayit({
        email: form.email,
        sifre: form.sifre,
        ad: form.ad,
        soyad: form.soyad,
        telefon: form.telefon || undefined,
      });
      girisYap(sonuc.token, sonuc.kullaniciId, sonuc.ad, sonuc.soyad, sonuc.email, sonuc.roller);
      router.push("/");
      router.refresh();
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Kayıt başarısız");
    } finally {
      setYukleniyor(false);
    }
  };

  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-md px-4 py-12">
        <div className="rounded-xl border bg-white p-8 shadow-sm">
          <h1 className="text-xl font-bold text-slate-800">Kayıt Ol</h1>
          <form onSubmit={handleSubmit} className="mt-6 space-y-4">
            <div className="grid gap-4 sm:grid-cols-2">
              <div>
                <label className="block text-sm font-medium text-slate-700">Ad</label>
                <input
                  type="text"
                  value={form.ad}
                  onChange={(e) => setForm((f) => ({ ...f, ad: e.target.value }))}
                  required
                  className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-700">Soyad</label>
                <input
                  type="text"
                  value={form.soyad}
                  onChange={(e) => setForm((f) => ({ ...f, soyad: e.target.value }))}
                  required
                  className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700">E-posta</label>
              <input
                type="email"
                value={form.email}
                onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
                required
                className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700">Şifre (min 6 karakter)</label>
              <input
                type="password"
                value={form.sifre}
                onChange={(e) => setForm((f) => ({ ...f, sifre: e.target.value }))}
                required
                minLength={6}
                className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700">Telefon (opsiyonel)</label>
              <input
                type="tel"
                value={form.telefon}
                onChange={(e) => setForm((f) => ({ ...f, telefon: e.target.value }))}
                className="mt-1 w-full rounded-lg border border-slate-300 px-3 py-2"
              />
            </div>
            {hata && <p className="text-sm text-red-600">{hata}</p>}
            <button
              type="submit"
              disabled={yukleniyor}
              className="w-full rounded-lg bg-slate-900 py-2 font-medium text-white hover:bg-slate-800 disabled:opacity-50"
            >
              {yukleniyor ? "Kayıt yapılıyor..." : "Kayıt Ol"}
            </button>
          </form>
          <p className="mt-4 text-center text-sm text-slate-600">
            Zaten hesabınız var mı?{" "}
            <Link href="/giris" className="font-medium text-slate-900 hover:underline">
              Giriş yapın
            </Link>
          </p>
        </div>
      </main>
    </div>
  );
}
