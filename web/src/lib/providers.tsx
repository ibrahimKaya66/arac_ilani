"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useState } from "react";
import { TokenRefreshProvider } from "@/components/TokenRefreshProvider";

export function Providers({ children }: { children: React.ReactNode }) {
  const [client] = useState(() => new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 60_000,
        retry: 1,
        retryDelay: 2000,
      },
    },
  }));
  return (
    <QueryClientProvider client={client}>
      <TokenRefreshProvider>{children}</TokenRefreshProvider>
    </QueryClientProvider>
  );
}
