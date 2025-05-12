using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public class PenarikanCustomer
    {
        private static PenarikanState currentState = PenarikanState.MEMASUKKAN_DATA;

        public static void StateBasedPenarikanCustomer()
        {
            Console.WriteLine("=== Fitur Penarikan Keuntungan (Customer) ===");

            while (true)
            {
                switch (currentState)
                {
                    case PenarikanState.MEMASUKKAN_DATA:
                        Console.Write("\nNomor Rekening: ");
                        string norek = Console.ReadLine();

                        Console.Write("Jumlah Penarikan: ");
                        string jumlah = Console.ReadLine();

                        currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.SUBMIT);
                        currentState = ValidasiData(norek, jumlah);
                        break;

                    case PenarikanState.BERHASIL:
                        Console.WriteLine("Penarikan berhasil dikirim!");
                        return;

                    case PenarikanState.GAGAL:
                        Console.WriteLine("Penarikan gagal. Coba lagi.");
                        Console.Write("Ketik 'retry' untuk ulang: ");
                        string retry = Console.ReadLine();
                        if (retry.ToLower() == "retry")
                        {
                            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.RETRY);
                        }
                        break;
                }
            }
        }

        private static PenarikanState ValidasiData(string rekening, string jumlah)
        {
            if (string.IsNullOrWhiteSpace(rekening) || rekening.Length < 5)
            {
                Console.WriteLine("Nomor rekening tidak valid.");
                return StateBasedPenarikan.GetNextState(PenarikanState.VALIDASI, PenarikanTrigger.INVALID);
            }

            if (!decimal.TryParse(jumlah, out decimal nominal) || nominal <= 0)
            {
                Console.WriteLine("Jumlah penarikan tidak valid.");
                return StateBasedPenarikan.GetNextState(PenarikanState.VALIDASI, PenarikanTrigger.INVALID);
            }

            decimal saldo = 100000;
            if (nominal > saldo)
            {
                Console.WriteLine("Saldo tidak cukup.");
                return StateBasedPenarikan.GetNextState(PenarikanState.VALIDASI, PenarikanTrigger.INVALID);
            }

            return StateBasedPenarikan.GetNextState(PenarikanState.VALIDASI, PenarikanTrigger.VALID);
        }
    }
}
