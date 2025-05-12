using Microsoft.AspNetCore.Identity.Data;
using modelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TugasBesar_KPL_2425_Kelompok_4;
using TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule;

namespace TugasBesar_KPL_2425_Kelompok_4.UserProgram
{
    class Admin
    {
        public static void adminProgram()
        {
            try
            {
                string login = "Edsel";
                int menuInput = 9999;
                while (menuInput != 5)
                {
                    Menu.menuAdmin(login);
                    menuInput = Convert.ToInt32(Console.ReadLine());
                    switch (menuInput)
                    {
                        case 1:
                            Console.WriteLine("=== Menjadwalkan Pengambilan Sampah ===");

                            Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
                            if (!DateOnly.TryParse(Console.ReadLine(), out var tanggal))
                            {
                                Console.WriteLine("Format tanggal tidak valid.");
                                return;
                            }

                            var jenisValues = Enum.GetValues<JenisSampah>();
                            Console.WriteLine("Pilih jenis sampah (pisahkan dengan koma):");
                            for (int i = 0; i < jenisValues.Length; i++)
                                Console.WriteLine($" {i}: {jenisValues[i]}");

                            Console.Write("Nomor jenis (e.g. 0,2,4): ");
                            var inputIndices = Console.ReadLine();
                            var indices = inputIndices?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(s => s.Trim())
                                                      .Where(s => int.TryParse(s, out _))
                                                      .Select(int.Parse)
                                                      .ToList() ?? new List<int>();

                            var jenisList = new List<JenisSampah>();
                            foreach (var idx in indices)
                            {
                                if (idx >= 0 && idx < jenisValues.Length)
                                    jenisList.Add(jenisValues[idx]);
                                else
                                    Console.WriteLine($"Index {idx} tidak valid, diabaikan.");
                            }

                            if (jenisList.Count == 0)
                            {
                                Console.WriteLine("Tidak ada jenis sampah yang dipilih.");
                                return;
                            }

                            Console.Write("Masukkan nama kurir: ");
                            var namaKurir = Console.ReadLine() ?? string.Empty;
                            Console.Write("Masukkan area pengambilan: ");
                            var area = Console.ReadLine() ?? string.Empty;

                            try
                            {
                                jadwalService.CreateAndSendJadwal(tanggal, jenisList, namaKurir, area);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 2:

                            break;
                        default:
                            Console.WriteLine("Pilihan tidak valid");
                            break;
                    }
                }
            } catch (Exception ex)
            {

            }
            
        }

    }
}
