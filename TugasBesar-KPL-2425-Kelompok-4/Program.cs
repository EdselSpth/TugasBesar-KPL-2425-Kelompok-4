using TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule;

namespace TugasBesar_KPL_2425_Kelompok_4
{
        class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ReGreen");
            Console.WriteLine("Kelompok 4");
            Console.WriteLine("Edsel Septa Haryanto - 103022300016");
            Console.WriteLine("Gusti Agung Raka Darma Putra Kepakisan - 103022300088");
            Console.WriteLine("Deru Khairan Djuharianto - 103022300101");
            Console.WriteLine("Reza Indra Maulana - 103022300109");
            Console.WriteLine("Abdul Azis Saepurohmat - 103022300092");
            Console.WriteLine("Tio Funny Tinambunan - 103022330036");
            Console.WriteLine("=====================================\n");

            Console.Write("Masukkan nama area: ");
            string inputArea = Console.ReadLine();


            configPendaftaraanArea areaBaru = new configPendaftaraanArea();
            areaBaru.area = inputArea;


            areaBaru.saveArea();

            Console.Write("Masukkan nama pengguna: ");
            string namaPengguna = Console.ReadLine();

            Console.Write("Masukkan jadwal penjemputan (yyyy-MM-dd HH:mm): ");
            DateTime jadwal;
            while (!DateTime.TryParse(Console.ReadLine(), out jadwal))
            {
                Console.Write("Format salah. Masukkan lagi (yyyy-MM-dd HH:mm): ");
            }

            Console.Write("Masukkan keterangan tambahan (opsional): ");
            string keterangan = Console.ReadLine();


            configPendaftaranPenjemputan<string> pendaftaran = new configPendaftaranPenjemputan<string>
            {
                namaPengguna = namaPengguna,
                Area = areaBaru,
                Jadwal = jadwal,
                KeteranganTambahan = keterangan
            };


            pendaftaran.Simpan();
            Console.WriteLine("\nTekan Enter untuk keluar...");
            Console.ReadLine();
        }
    }
}