import Link from "next/link";
import { Header } from "@/components/Header";
import { IlanListe } from "@/components/IlanListe";
import { HeroCta } from "@/components/HeroCta";

export const metadata = {
  title: "Araç İlan - Türkiye'nin Araç Alım Satım Platformu",
  description: "Otomobil, SUV ve Pickup ilanları. Güvenli alım satım.",
};

export default function HomePage() {
  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <HeroCta />
        <section className="mt-12 rounded-2xl border border-slate-700/50 bg-slate-800/50 p-6 backdrop-blur sm:p-8">
          <IlanListe dark />
        </section>
      </main>
    </div>
  );
}
