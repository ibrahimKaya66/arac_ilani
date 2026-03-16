"use client";

import { useEffect, useRef } from "react";
import { useAuthStore } from "@/lib/auth-store";
import { jwtExpAl } from "@/lib/api";

/** Token süresi 1 saat - sitede iken 15 dk kala yenile */
const YENILEME_EŞİĞİ_SANIYE = 15 * 60;
const KONTROL_ARALIĞI_MS = 15 * 60 * 1000;

export function TokenRefreshProvider({ children }: { children: React.ReactNode }) {
  const token = useAuthStore((s) => s.token);
  const tokenYenile = useAuthStore((s) => s.tokenYenile);
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);

  useEffect(() => {
    if (!token) {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
        intervalRef.current = null;
      }
      return;
    }

    const kontrolEt = async () => {
      const exp = jwtExpAl(token);
      if (!exp) return;
      const kalan = exp - Math.floor(Date.now() / 1000);
      if (kalan < YENILEME_EŞİĞİ_SANIYE) {
        await tokenYenile();
      }
    };

    kontrolEt();
    intervalRef.current = setInterval(kontrolEt, KONTROL_ARALIĞI_MS);
    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, [token, tokenYenile]);

  return <>{children}</>;
}
