using System;
using System.Collections.Generic;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public static class PenarikanAdmin
    {
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
            if (PenarikanCustomer.RiwayatPenarikan.Count == 0)
            {
                Console.WriteLine("Belum ada permintaan penarikan dari customer.");
                return;
            }

            // Ambil data penarikan terakhir
            PenarikanData dataTerakhir = PenarikanCustomer.RiwayatPenarikan[^1];
            PembayaranInfo info = PembayaranTable[dataTerakhir.MetodePembayaran];
            decimal totalDiterima = dataTerakhir.Nominal - info.BiayaAdmin;

            Console.WriteLine("\n=== Fitur Penarikan Keuntungan (Admin) ===");
            Console.WriteLine("Saldo                : 200000");
            Console.WriteLine($"Nomor Rekening       : {dataTerakhir.NomorRekening}");
            Console.WriteLine($"Metode Pembayaran    : {info.Deskripsi}");
            Console.WriteLine($"Biaya Admin          : {info.BiayaAdmin}");
            Console.WriteLine($"Minimal Penarikan    : {info.MinimalPenarikan}");
            Console.WriteLine($"Jumlah Diajukan      : {dataTerakhir.Nominal}");
            Console.WriteLine($"Jumlah Diterima      : {totalDiterima}");

            Console.WriteLine("\nPermintaan penarikan menunggu approval...");
            Console.WriteLine("1. Setujui");
            Console.WriteLine("2. Tolak");

            string approval = Console.ReadLine();

            if (approval == "1")
            {
                Console.WriteLine("Permintaan penarikan disetujui.\n");
                // Proses state transition dari MENUNGGU_APPROVAL ke DITERIMA, lalu ke BERHASIL
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.APPROVE);
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.TRANSFER);
            }
            else
            {
                Console.WriteLine("Permintaan penarikan ditolak.\n");
                // Jika ditolak, state ke DITOLAK
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.REJECT);
            }
        }

        public static void TampilkanDataPenarikanCustomer()
        {
            Console.WriteLine("\n=== Data Penarikan dari Customer ===");

            if (PenarikanCustomer.RiwayatPenarikan.Count == 0)
            {
                Console.WriteLine("Belum ada data penarikan.\n");
                return;
            }

            foreach (var data in PenarikanCustomer.RiwayatPenarikan)
            {
                Console.WriteLine($"Nominal: {data.Nominal}, No Rekening: {data.NomorRekening}, Metode: {data.MetodePembayaran}");
            }
        }
    }
}