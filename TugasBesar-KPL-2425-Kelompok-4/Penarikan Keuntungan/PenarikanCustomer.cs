using System;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan; 
namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public class PenarikanCustomer
    {
        public static PenarikanState currentState = PenarikanState.MEMASUKKAN_DATA;

        public static void StateBasedPenarikanCustomer()
        {
            Console.WriteLine("=== Fitur Penarikan Keuntungan (Customer) ===");

            Console.Write("\nNomor Rekening: ");
            string norek = Console.ReadLine();

            Console.Write("Jumlah Penarikan: ");
            string jumlah = Console.ReadLine();

      
            Console.WriteLine($"\nNomor Rekening: {norek}");
            Console.WriteLine($"Jumlah Penarikan: {jumlah}");

     
            Console.WriteLine("\nPilih metode pembayaran:");
            foreach (var pembayaran in Enum.GetValues(typeof(Pembayaran)))
            {
                Console.WriteLine($"{(int)pembayaran}. {pembayaran}");
            }

            Console.Write("Masukkan nomor metode pembayaran yang dipilih: ");
            int bankChoice = int.Parse(Console.ReadLine());

            Pembayaran selectedBank = (Pembayaran)bankChoice;
            PembayaranInfo bankInfo = PenarikanAdmin.PembayaranTable[selectedBank];

            decimal nominal = 0;
            if (!decimal.TryParse(jumlah, out nominal) || nominal < bankInfo.MinimalPenarikan)
            {
                Console.WriteLine("Jumlah penarikan kurang dari minimal.");
                Console.ReadKey();
                return;
            }

            decimal totalDiterima = nominal - bankInfo.BiayaAdmin;
            if (totalDiterima <= 0)
            {
                Console.WriteLine("Jumlah penarikan tidak mencukupi setelah potongan biaya admin.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nMetode pembayaran terpilih : {selectedBank}");
            Console.WriteLine($"Biaya admin: {bankInfo.BiayaAdmin}");
            Console.WriteLine($"Jumlah yang akan diterima setelah potongan biaya admin: {totalDiterima}");


            currentState = PenarikanState.MENUNGGU_APPROVAL;
            //PenarikanAdmin.ProsesPenarikan(ref currentState, selectedBank, nominal);

            //if (currentState == PenarikanState.BERHASIL)
            //{
            //    Console.WriteLine("\nPenarikan berhasil dikirim!");
            //}
            //else if (currentState == PenarikanState.DITOLAK)
            //{
            //    Console.WriteLine("\nPenarikan ditolak oleh admin.");
            //}

            Console.WriteLine("\nTekan sembarang tombol untuk keluar...");
            Console.ReadKey();
        }
    }
}