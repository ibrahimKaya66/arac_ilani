import { Header } from "@/components/Header";
import { IlanListe } from "@/components/IlanListe";

export const metadata = {
  title: "İlanlar - Araç İlan",
  description: "Tüm araç ilanları",
};

export default function IlanlarPage() {
  return (
    <div className="min-h-screen bg-slate-50">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-slate-800">Tüm İlanlar</h1>
        <IlanListe />
      </main>
    </div>
  );
}
