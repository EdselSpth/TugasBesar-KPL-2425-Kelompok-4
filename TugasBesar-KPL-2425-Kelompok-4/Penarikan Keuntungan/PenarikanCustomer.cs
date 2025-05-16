using System;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan;
namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public static class PenarikanCustomer
    {
        public static List<PenarikanData> RiwayatPenarikan = new();

        public static readonly Dictionary<Pembayaran, PembayaranInfo> PembayaranTable = new()
        {
            { Pembayaran.Tunai, new PembayaranInfo("Pembayaran menggunakan uang tunai", 0, 50000) },
            { Pembayaran.Bca, new PembayaranInfo("Transfer melalui BCA", 5000, 50000) },
            { Pembayaran.Bni, new PembayaranInfo("Transfer melalui BNI", 5000, 50000) },
            { Pembayaran.Mandiri, new PembayaranInfo("Transfer melalui Mandiri", 5000, 50000) },
            { Pembayaran.Bri, new PembayaranInfo("Transfer melalui BRI", 5000, 50000) },
            { Pembayaran.ShopeePay, new PembayaranInfo("Pembayaran melalui ShopeePay", 1000, 50000) },
            { Pembayaran.Gopay, new PembayaranInfo("Pembayaran melalui Gopay", 1000, 50000) },
            { Pembayaran.Dana, new PembayaranInfo("Pembayaran melalui Dana", 1000, 50000) },
        };

        public static void ProsesPenarikan(ref PenarikanState currentState)
        {
            Console.WriteLine("\n=== Fitur Penarikan Keuntungan ===");

            decimal nominal = 0;
            while (true)
            {
                Console.Write("Masukkan jumlah penarikan: ");
                if (decimal.TryParse(Console.ReadLine(), out nominal) && nominal > 0)
                    break;
                Console.WriteLine("Input nominal tidak valid. Silakan coba lagi.");
            }

            Console.Write("Masukkan nomor rekening: ");
            string rekening = Console.ReadLine();

            Console.WriteLine("\nMetode Pembayaran:");
            Console.WriteLine("1. Tunai\n2. BCA\n3. BNI\n4. Mandiri\n5. BRI\n6. ShopeePay\n7. GoPay\n8. Dana");

            int metode = 0;
            while (true)
            {
                Console.Write("Pilih metode (1-8): ");
                if (int.TryParse(Console.ReadLine(), out metode) && metode >= 1 && metode <= 8)
                    break;
                Console.WriteLine("Input metode tidak valid. Silakan pilih angka 1 sampai 8.");
            }

            Pembayaran selectedMethod = (Pembayaran)(metode - 1);

            PembayaranInfo info = PembayaranTable[selectedMethod];

            if (nominal < info.MinimalPenarikan)
            {
                Console.WriteLine($"Minimal penarikan untuk metode ini adalah {info.MinimalPenarikan}.\n");
                return;
            }

            decimal totalDiterima = nominal - info.BiayaAdmin;
            Console.WriteLine($"Jumlah yang akan diterima setelah dipotong biaya admin ({info.BiayaAdmin}): {totalDiterima}");

            // Buat nyimpen PenarikanDatanya
            RiwayatPenarikan.Add(new PenarikanData(rekening, nominal, selectedMethod));
            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.SUBMIT);

            Console.WriteLine($"Status penarikan sekarang: {currentState}");
            Console.WriteLine("\nMemproses penarikan, mohon tunggu...");
            Thread.Sleep(2000);

            currentState = PenarikanState.BERHASIL;
            Console.WriteLine("\nPenarikan berhasil dikirim ke rekening Anda!");
            Console.WriteLine($"Total diterima: {totalDiterima} ({selectedMethod})");

            Console.WriteLine("\nTekan sembarang tombol untuk keluar...");
            Console.ReadKey();
        }
    }
}