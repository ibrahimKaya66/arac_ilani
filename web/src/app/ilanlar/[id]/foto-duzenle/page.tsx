"use client";

import { useEffect, useRef, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import Link from "next/link";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuthStore, useAuthHydrated } from "@/lib/auth-store";
import { useShallow } from "zustand/react/shallow";
import { Header } from "@/components/Header";

export default function FotoDuzenlePage() {
  const params = useParams();
  const router = useRouter();
  const queryClient = useQueryClient();
  const id = Number(params.id);
  const hydrated = useAuthHydrated();
  const { token, kullaniciId } = useAuthStore(useShallow((s) => ({ token: s.token, kullaniciId: s.kullaniciId })));
  const [gorselYollari, setGorselYollari] = useState<string[]>([]);
  const [expertizYolu, setExpertizYolu] = useState<string | null>(null);
  const [hata, setHata] = useState<string | null>(null);
  const gorselInputRef = useRef<HTMLInputElement>(null);
  const expertizInputRef = useRef<HTMLInputElement>(null);

  const { data, isLoading, error } = useQuery({
    queryKey: ["ilan", id],
    queryFn: () => api.ilanDetay(id),
    enabled: !Number.isNaN(id) && !!token,
  });

  const { data: ilanHakkim } = useQuery({
    queryKey: ["ilanHakkim"],
    queryFn: () => api.ilanHakkim(token!),
    enabled: !!token,
  });

  useEffect(() => {
    if (data) {
      setGorselYollari(data.gorselYollari ?? []);
    }
  }, [data]);

  const guncelleMutation = useMutation({
    mutationFn: () => api.ilanGuncelle(id, {
      gorselYollari,
      ...(expertizYolu != null && { expertizGorselYolu: expertizYolu }),
    }, token!),
    onSuccess: async () => {
      await queryClient.refetchQueries({ queryKey: ["ilan", id] });
      router.push(`/ilanlar/${id}`);
    },
    onError: (err: Error) => setHata(err.message),
  });

  const maksFotograf = ilanHakkim?.maksFotograf ?? 8;

  const gorselYukle = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files?.length || !token) return;
    if (gorselYollari.length + files.length > maksFotograf) {
      setHata(`En fazla ${maksFotograf} fotoğraf yükleyebilirsiniz.`);
      e.target.value = "";
      return;
    }
    setHata(null);
    try {
      const yollar: string[] = [];
      for (let i = 0; i < files.length; i++) {
        const yol = await api.gorselYukle(files[i], token);
        yollar.push(yol);
      }
      setGorselYollari((prev) => [...prev, ...yollar]);
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Yükleme hatası");
    }
    e.target.value = "";
  };

  const expertizYukle = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !token) return;
    setHata(null);
    try {
      const yol = await api.expertizGorseliYukle(file, token);
      setExpertizYolu(yol);
    } catch (err) {
      setHata(err instanceof Error ? err.message : "Yükleme hatası");
    }
    e.target.value = "";
  };

  const gorselKaldir = (index: number) => {
    setGorselYollari((prev) => prev.filter((_, i) => i !== index));
  };

  if (!hydrated) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <p className="text-slate-400">Yükleniyor...</p>
      </div>
    );
  }

  if (!token) {
    router.push("/giris");
    return null;
  }

  if (isLoading || !data) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-2xl px-4 py-8">
          <div className="h-64 animate-pulse rounded-lg bg-slate-700" />
        </main>
      </div>
    );
  }

  if (error || (data.kullaniciId && data.kullaniciId !== kullaniciId)) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
        <Header />
        <main className="mx-auto max-w-2xl px-4 py-8">
          <p className="text-red-400">Bu ilanın fotoğraflarını düzenleyemezsiniz.</p>
          <Link href={`/ilanlar/${id}`} className="mt-4 inline-block text-slate-300 hover:text-white">← İlana dön</Link>
        </main>
      </div>
    );
  }

  const gorselUrl = (path: string) => `${process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5094"}${path}`;

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-2xl px-4 py-8">
        <Link href={`/ilanlar/${id}`} className="mb-4 inline-block text-sm text-slate-300 hover:text-white">← İlana dön</Link>
        <h1 className="mb-6 text-2xl font-bold text-white">Fotoğraf Düzenle</h1>
        <div className="rounded-xl border border-slate-700/50 bg-slate-800/80 p-6 shadow-lg backdrop-blur">
          <div className="space-y-4">
            <div>
              <h3 className="font-medium text-white">Araç Fotoğrafları</h3>
              <input ref={gorselInputRef} type="file" accept="image/*" multiple onChange={gorselYukle} className="hidden" />
              <button
                type="button"
                onClick={() => gorselInputRef.current?.click()}
                className="mt-2 rounded-lg border border-dashed border-slate-600 px-4 py-2 text-sm text-slate-400 hover:bg-slate-700/50"
              >
                + Fotoğraf ekle (en fazla {maksFotograf})
              </button>
              <p className="mt-1 text-sm text-slate-400">{gorselYollari.length}/{maksFotograf} fotoğraf</p>
              <div className="mt-3 grid grid-cols-3 gap-2">
                {gorselYollari.map((yol, i) => (
                  <div key={i} className="relative">
                    <img src={gorselUrl(yol)} alt="" className="h-24 w-full rounded object-cover" />
                    <button
                      type="button"
                      onClick={() => gorselKaldir(i)}
                      className="absolute right-1 top-1 rounded bg-red-600/90 px-2 py-0.5 text-xs text-white hover:bg-red-500"
                    >
                      Kaldır
                    </button>
                  </div>
                ))}
              </div>
            </div>
            {data.hasarDurumu === "ExpertizVar" && (
              <div>
                <h3 className="font-medium text-white">Expertiz Görseli</h3>
                <input ref={expertizInputRef} type="file" accept="image/*" onChange={expertizYukle} className="hidden" />
                <button
                  type="button"
                  onClick={() => expertizInputRef.current?.click()}
                  className="mt-2 rounded-lg border border-dashed border-slate-600 px-4 py-2 text-sm text-slate-400 hover:bg-slate-700/50"
                >
                  {expertizYolu ? "Expertiz değiştir" : "+ Expertiz yükle"}
                </button>
              </div>
            )}
            {hata && <p className="text-sm text-red-400">{hata}</p>}
            <button
              onClick={() => guncelleMutation.mutate()}
              disabled={guncelleMutation.isPending}
              className="w-full rounded-lg bg-emerald-600 py-2 font-medium text-white hover:bg-emerald-500 disabled:opacity-50"
            >
              {guncelleMutation.isPending ? "Kaydediliyor..." : "Kaydet"}
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}
