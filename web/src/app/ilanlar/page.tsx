import { Header } from "@/components/Header";
import { IlanlarIcerik } from "./IlanlarIcerik";

export const metadata = {
  title: "İlanlar - Araç İlan",
  description: "Tüm araç ilanları",
};

export default function IlanlarPage() {
  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-900 via-slate-800 to-slate-900">
      <Header />
      <main className="mx-auto max-w-6xl px-4 py-8">
        <h1 className="mb-6 text-2xl font-bold text-white">Tüm İlanlar</h1>
        <IlanlarIcerik />
      </main>
    </div>
  );
}
