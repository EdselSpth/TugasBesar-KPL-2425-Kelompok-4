using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    class PenarikanAdmin
    {
        private static PenarikanState currentState = PenarikanState.MENUNGGU_APPROVAL;

        public static void StateBasedPenarikanAdmin()
        {
            Console.WriteLine("=== Fitur Penarikan Keuntungan (Admin) ===");

            while (true)
            {
                switch (currentState)
                {
                    case PenarikanState.MENUNGGU_APPROVAL:
                        Console.WriteLine("\nPermintaan penarikan menunggu approval...");
                        Console.WriteLine("1. Setujui");
                        Console.WriteLine("2. Tolak");
                        string approval = Console.ReadLine();

                        if (approval == "1")
                        {
                            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.APPROVE);
                        }
                        else if (approval == "2")
                        {
                            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.REJECT);
                        }
                        break;

                    case PenarikanState.DITERIMA:
                        Console.WriteLine("Permintaan penarikan disetujui! Transaksi berhasil.");
                        currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.TRANSFER);
                        return;

                    case PenarikanState.DITOLAK:
                        Console.WriteLine("Permintaan penarikan ditolak.");
                        return;
                }
            }
        }
    }
}
