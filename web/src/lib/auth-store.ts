import { create } from "zustand";
import { persist } from "zustand/middleware";

interface AuthState {
  token: string | null;
  kullaniciId: string | null;
  ad: string;
  soyad: string;
  email: string;
  roller: string[];
  girisYap: (token: string, kullaniciId: string, ad: string, soyad: string, email: string, roller: string[]) => void;
  cikisYap: () => void;
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
      girisYap: (token, kullaniciId, ad, soyad, email, roller) =>
        set({ token, kullaniciId, ad, soyad, email, roller, girisliMi: true }),
      cikisYap: () => set({ token: null, kullaniciId: null, ad: "", soyad: "", email: "", roller: [], girisliMi: false }),
    }),
    { name: "arac-ilan-auth" }
  )
);
