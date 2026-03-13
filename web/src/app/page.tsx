import { Header } from "@/components/Header";
import { IlanListe } from "@/components/IlanListe";

export const metadata = {
  title: "Araç İlan - Türkiye'nin Araç Alım Satım Platformu",
  description: "Otomobil, SUV ve Pickup ilanları. Güvenli alım satım.",
};

export default function HomePage() {
  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <section className="mb-8 rounded-xl bg-gradient-to-r from-slate-800 to-slate-700 p-8 text-white">
          <h1 className="text-2xl font-bold md:text-3xl">Araç Alım Satım Platformu</h1>
          <p className="mt-2 text-slate-200">Otomobil, SUV ve Pickup ilanları. Expertiz ve hasar bilgisi ile güvenli alım satım.</p>
        </section>
        <IlanListe />
      </main>
    </div>
  );
}
