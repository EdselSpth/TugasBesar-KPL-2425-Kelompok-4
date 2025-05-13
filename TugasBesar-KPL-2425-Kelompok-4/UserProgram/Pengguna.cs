using JadwalAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.UserProgram
{
    class Pengguna
    {
        public static void PenggunaProgram()
        {
            try
            {
                string username = "Rey";
                int inputUser = 999;
                while (inputUser != 5)
                {
                    Menu.menuUser(username);
                    inputUser = Convert.ToInt32(Console.ReadLine());
                    switch (inputUser)
                    {
                        case 1:
                            jadwalService.ViewJadwal();
                            break;
                        case 2:
                            PenarikanCustomer.StateBasedPenarikanCustomer();
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            Console.WriteLine("Terima kasih sudah menggunakan aplikasi");
                            break;
                        default:
                            Console.WriteLine("Tidak pilihan menu itu");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
