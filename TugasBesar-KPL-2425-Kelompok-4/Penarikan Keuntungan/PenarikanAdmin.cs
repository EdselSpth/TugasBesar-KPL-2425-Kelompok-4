using System;
using System.Collections.Generic;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public class PenarikanAdmin
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

        public static void ProsesPenarikan(ref PenarikanState currentState, Pembayaran selectedMethod, decimal nominal)
        {
            PembayaranInfo info = PembayaranTable[selectedMethod];
            decimal totalDiterima = nominal - info.BiayaAdmin;

            Console.WriteLine("\n=== Fitur Penarikan Keuntungan (Admin) ===");
            Console.WriteLine("Saldo                : 200000");
            Console.WriteLine($"Metode Pembayaran    : {info.Deskripsi}");
            Console.WriteLine($"Biaya Admin          : {info.BiayaAdmin}");
            Console.WriteLine($"Minimal Penarikan    : {info.MinimalPenarikan}");
            Console.WriteLine($"Jumlah Diajukan      : {nominal}");
            Console.WriteLine($"Jumlah Diterima      : {totalDiterima}");

            Console.WriteLine("\nPermintaan penarikan menunggu approval...");
            Console.WriteLine("1. Setujui");
            Console.WriteLine("2. Tolak");

            string approval = Console.ReadLine();

            if (approval == "1")
            {
                Console.WriteLine("Permintaan penarikan disetujui.");
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.APPROVE);
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.TRANSFER);
            }
            else
            {
                Console.WriteLine("Permintaan penarikan ditolak.");
                currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.REJECT);
            }
        }
    }
}