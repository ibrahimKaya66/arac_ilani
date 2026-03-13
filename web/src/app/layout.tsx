import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import { Providers } from "@/lib/providers";
import "./globals.css";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

const siteUrl = process.env.NEXT_PUBLIC_SITE_URL ?? "https://aracilan.com";

export const metadata: Metadata = {
  metadataBase: new URL(siteUrl),
  title: {
    default: "Araç İlan - Türkiye'nin Araç Alım Satım Platformu",
    template: "%s | Araç İlan",
  },
  description: "Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım. Türkiye'nin güvenilir araç ilan platformu.",
  keywords: ["araç ilan", "otomobil", "SUV", "pickup", "ikinci el araç", "sahibinden", "araç alım satım"],
  authors: [{ name: "Araç İlan" }],
  openGraph: {
    type: "website",
    locale: "tr_TR",
    url: siteUrl,
    siteName: "Araç İlan",
    title: "Araç İlan - Türkiye'nin Araç Alım Satım Platformu",
    description: "Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım.",
  },
  twitter: {
    card: "summary_large_image",
    title: "Araç İlan - Türkiye'nin Araç Alım Satım Platformu",
    description: "Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım.",
  },
  robots: { index: true, follow: true },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="tr">
      <body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}
