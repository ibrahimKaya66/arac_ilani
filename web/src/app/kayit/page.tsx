"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { api } from "@/lib/api";
import { useAuthStore } from "@/lib/auth-store";
import { Header } from "@/components/Header";

const SIFRE_KURALLARI = [
  { id: "min6", metin: "En az 6 karakter", test: (s: string) => s.length >= 6 },
  { id: "buyuk", metin: "En az 1 büyük harf", test: (s: string) => /[A-Z]/.test(s) },
  { id: "kucuk", metin: "En az 1 küçük harf", test: (s: string) => /[a-z]/.test(s) },
  { id: "rakam", metin: "En az 1 rakam", test: (s: string) => /\d/.test(s) },
  { id: "ozel", metin: "En az 1 noktalama (. , ! ? vb.)", test: (s: string) => /[.!@#$%^&*(),;:'"<>?/[\]{}|\\_\-+=`~]/.test(s) },
];

function SifreGosterGizle({ goster, toggle }: { goster: boolean; toggle: () => void }) {
  return (
    <button
      type="button"
      onClick={toggle}
      className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-white"
      tabIndex={-1}
      aria-label={goster ? "Şifreyi gizle" : "Şifreyi göster"}
    >
      {goster ? (
        <svg className="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
        </svg>
      ) : (
        <svg className="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
        </svg>
      )}
    </button>
  );
}

export default function KayitPage() {
  const router = useRouter();
  const girisYap = useAuthStore((s) => s.girisYap);
  const [form, setForm] = useState({ email: "", sifre: "", ad: "", soyad: "", telefon: "" });
  const [sifreGoster, setSifreGoster] = useState(false);
  const [hata, setHata] = useState("");
  const [yukleniyor, setYukleniyor] = useState(false);

  const sifreGecerli = SIFRE_KURALLARI.every((k) => k.test(form.sifre));
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const emailGecerli = !form.email || emailRegex.test(form.email);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setHata("");
    if (!emailGecerli) {
      setHata("Geçerli bir e-posta adresi girin.");
      return;
    }
    if (!sifreGecerli) {
      setHata("Şifre tüm kuralları karşılamalıdır.");
      return;
    }
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
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-md px-4 py-12">
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-8 shadow-lg backdrop-blur">
          <h1 className="text-2xl font-bold text-white">Kayıt Ol</h1>
          <form onSubmit={handleSubmit} className="mt-6 space-y-4">
            <div className="grid gap-4 sm:grid-cols-2">
              <div>
                <label className="block text-sm font-medium text-slate-300">Ad</label>
                <input
                  type="text"
                  value={form.ad}
                  onChange={(e) => setForm((f) => ({ ...f, ad: e.target.value }))}
                  required
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white placeholder-slate-500 focus:border-emerald-500 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-slate-300">Soyad</label>
                <input
                  type="text"
                  value={form.soyad}
                  onChange={(e) => setForm((f) => ({ ...f, soyad: e.target.value }))}
                  required
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white placeholder-slate-500 focus:border-emerald-500 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300">E-posta</label>
              <input
                type="email"
                value={form.email}
                onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
                required
                className={`mt-1 w-full rounded-lg border bg-slate-900/50 px-3 py-2 text-white placeholder-slate-500 focus:outline-none focus:ring-1 ${
                  form.email && !emailGecerli
                    ? "border-red-500 focus:border-red-500 focus:ring-red-500"
                    : "border-slate-600 focus:border-emerald-500 focus:ring-emerald-500"
                }`}
              />
              {form.email && !emailGecerli && (
                <p className="mt-1 text-xs text-red-400">Geçerli bir e-posta adresi girin (örn: ornek@email.com)</p>
              )}
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300">Şifre</label>
              <div className="relative">
                <input
                  type={sifreGoster ? "text" : "password"}
                  value={form.sifre}
                  onChange={(e) => setForm((f) => ({ ...f, sifre: e.target.value }))}
                  required
                  className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 pr-10 text-white placeholder-slate-500 focus:border-emerald-500 focus:outline-none focus:ring-1 focus:ring-emerald-500"
                />
                <SifreGosterGizle goster={sifreGoster} toggle={() => setSifreGoster((g) => !g)} />
              </div>
              <div className="mt-2 space-y-1 rounded-lg bg-slate-900/50 p-3 text-xs text-slate-400">
                <p className="font-medium text-slate-300">Şifre kuralları:</p>
                {SIFRE_KURALLARI.map((k) => (
                  <div key={k.id} className={k.test(form.sifre) ? "text-emerald-400" : "text-slate-500"}>
                    {k.test(form.sifre) ? "✓" : "○"} {k.metin}
                  </div>
                ))}
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-300">Telefon (opsiyonel)</label>
              <input
                type="tel"
                value={form.telefon}
                onChange={(e) => setForm((f) => ({ ...f, telefon: e.target.value }))}
                className="mt-1 w-full rounded-lg border border-slate-600 bg-slate-900/50 px-3 py-2 text-white placeholder-slate-500 focus:border-emerald-500 focus:outline-none focus:ring-1 focus:ring-emerald-500"
              />
            </div>
            {hata && (
              <div className="rounded-lg bg-red-500/20 p-3 text-sm text-red-300">
                {hata}
              </div>
            )}
            <button
              type="submit"
              disabled={yukleniyor || !sifreGecerli || !emailGecerli}
              className="w-full rounded-lg bg-emerald-600 py-2.5 font-medium text-white hover:bg-emerald-500 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {yukleniyor ? "Kayıt yapılıyor..." : "Kayıt Ol"}
            </button>
            <div className="relative my-4">
              <div className="absolute inset-0 flex items-center">
                <div className="w-full border-t border-slate-600" />
              </div>
              <div className="relative flex justify-center text-sm">
                <span className="bg-slate-800/80 px-2 text-slate-400">veya</span>
              </div>
            </div>
            <a
              href={`${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094"}/api/kimlik/google?returnUrl=${encodeURIComponent(typeof window !== "undefined" ? `${window.location.origin}/auth/callback` : "http://localhost:3000/auth/callback")}`}
              className="flex w-full items-center justify-center gap-2 rounded-lg border border-slate-600 bg-slate-800/80 py-2.5 font-medium text-white hover:bg-slate-700"
            >
              <svg className="h-5 w-5" viewBox="0 0 24 24">
                <path fill="currentColor" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" />
                <path fill="currentColor" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" />
                <path fill="currentColor" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" />
                <path fill="currentColor" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" />
              </svg>
              Google ile Kayıt Ol
            </a>
          </form>
          <p className="mt-4 text-center text-sm text-slate-400">
            Zaten hesabınız var mı?{" "}
            <Link href="/giris" className="font-medium text-emerald-400 hover:text-emerald-300">
              Giriş yapın
            </Link>
          </p>
        </div>
      </main>
    </div>
  );
}
