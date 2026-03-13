import { useEffect, useState } from "react";
import { create } from "zustand";
import { persist } from "zustand/middleware";

const REMEMBER_KEY = "arac-ilan-remember";

function getRemember(): boolean {
  if (typeof window === "undefined") return true;
  const v = localStorage.getItem(REMEMBER_KEY);
  return v !== "false";
}

/** Beni hatırla seçimine göre localStorage veya sessionStorage kullanır */
const beniHatirlaStorage = {
  getItem: (name: string) => {
    if (typeof window === "undefined") return null;
    const storage = getRemember() ? localStorage : sessionStorage;
    return storage.getItem(name);
  },
  setItem: (name: string, value: string) => {
    if (typeof window === "undefined") return;
    const storage = getRemember() ? localStorage : sessionStorage;
    storage.setItem(name, value);
  },
  removeItem: (name: string) => {
    if (typeof window === "undefined") return;
    localStorage.removeItem(name);
    sessionStorage.removeItem(name);
  },
};

interface AuthState {
  token: string | null;
  kullaniciId: string | null;
  ad: string;
  soyad: string;
  email: string;
  roller: string[];
  girisYap: (token: string, kullaniciId: string, ad: string, soyad: string, email: string, roller: string[], beniHatirla?: boolean) => void;
  cikisYap: () => void;
  tokenYenile: () => Promise<boolean>;
  girisliMi: boolean;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      kullaniciId: null,
      ad: "",
      soyad: "",
      email: "",
      roller: [],
      girisliMi: false,
      girisYap: (token, kullaniciId, ad, soyad, email, roller, beniHatirla = true) => {
        if (typeof window !== "undefined") {
          localStorage.setItem(REMEMBER_KEY, beniHatirla ? "true" : "false");
        }
        set({ token, kullaniciId, ad, soyad, email, roller, girisliMi: true });
      },
      cikisYap: () => set({ token: null, kullaniciId: null, ad: "", soyad: "", email: "", roller: [], girisliMi: false }),
      tokenYenile: async () => {
        const { token } = useAuthStore.getState();
        if (!token) return false;
        const { tokenYenile: refresh } = await import("@/lib/api");
        const sonuc = await refresh(token);
        if (!sonuc) return false;
        useAuthStore.getState().girisYap(sonuc.token, sonuc.kullaniciId, sonuc.ad, sonuc.soyad, sonuc.email, sonuc.roller, true);
        return true;
      },
    }),
    { name: "arac-ilan-auth", storage: beniHatirlaStorage }
  )
);

/** Rehydration tamamlanana kadar bekler - yoksa giriş yapılmış olsa bile /giris'e atar */
export function useAuthHydrated(): boolean {
  const [hydrated, setHydrated] = useState(false);
  useEffect(() => {
    const store = useAuthStore as typeof useAuthStore & { persist?: { hasHydrated: () => boolean; onFinishHydration: (cb: () => void) => () => void } };
    const persistApi = store.persist;
    if (!persistApi) {
      setHydrated(true);
      return;
    }
    if (persistApi.hasHydrated()) {
      setHydrated(true);
      return;
    }
    const unsub = persistApi.onFinishHydration(() => setHydrated(true));
    return unsub;
  }, []);
  return hydrated;
}
