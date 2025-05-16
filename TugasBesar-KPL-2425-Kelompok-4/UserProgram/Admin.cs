using Microsoft.AspNetCore.Identity.Data;
using modelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TugasBesar_KPL_2425_Kelompok_4;
using TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace TugasBesar_KPL_2425_Kelompok_4.UserProgram
{
    class Admin
    {
        public static void adminProgram()
        {
            try
            {
                int menuInput = 9999;
                while (menuInput != 6)
                {                    
                    Menu.menuAdmin();
                    Console.Write("Pilih Menu : ");
                    menuInput = Convert.ToInt32(Console.ReadLine());
                    Menu.header();
                    switch (menuInput)
                    {
                        case 1:
                            createFlow();
                            break;
                        case 2:
                            jadwalService.ViewJadwal();
                            break;
                        case 3:
                            editFlow();
                            break;
                        case 4:
                            DeleteFlow();
                            break;
                        case 5:
                            PenarikanAdmin.TampilkanDataPenarikanCustomer();
                            PenarikanState state = PenarikanState.MEMASUKKAN_DATA;
                            PenarikanAdmin.ProsesPenarikan(ref state);
                            break;
                        case 6:
                            Console.WriteLine("Terima kasih sudah menggunakan aplikasi\n");
                            break;
                        default:
                            Console.WriteLine("Pilihan tidak valid.\n");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
        }
        static void createFlow()
        {
            Console.WriteLine("\n∘<<──────>>∘ TAMBAH JADWAL PENGAMBILAN SAMPAH ∘<<──────>>∘");
            Console.Write("Masukkan tanggal (yyyy-MM-dd): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var tanggal))
            {
                Console.WriteLine("Format tanggal tidak valid.\n");
                return;
            }

            // Pengecekan tanggal
            if (tanggal < DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("Tanggal tidak boleh berada di masa lalu.\n");
                return;
            }

            var jenisValues = Enum.GetValues<JenisSampah>();
            Console.WriteLine("Pilih jenis sampah (pisahkan dengan koma):");
            for (int i = 0; i < jenisValues.Length; i++)
                Console.WriteLine($" {i}: {jenisValues[i]}");

            Console.Write("Nomor jenis (e.g. 0,2,4): ");
            var jenisList = Console.ReadLine()?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse)
                .Where(idx => idx >= 0 && idx < jenisValues.Length)
                .Select(idx => jenisValues[idx])
                .ToList() ?? new List<JenisSampah>();

            if (!jenisList.Any())
            {
                Console.WriteLine("Daftar jenis sampah tidak boleh kosong.\n");
                return;
            }

            Console.Write("Masukkan nama kurir: ");
            var namaKurir = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(namaKurir))
            {
                Console.WriteLine("Nama kurir tidak boleh kosong.\n");
                return;
            }

            Console.Write("Masukkan area pengambilan: ");
            var area = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(area))
            {
                Console.WriteLine("Area pengambilan tidak boleh kosong.\n");
                return;
            }

            var kurirObj = new pengguna(namaKurir, JenisPengguna.Kurir);
            if (kurirObj.peran != JenisPengguna.Kurir)
            {
                Console.WriteLine("Hanya peran Kurir yang dapat membuat jadwal.\n");
                return;
            }

            // Validasi hari pengambilan berdasarkan rules
            var invalid = jenisList.Where(j => !rulesJadwal.pengambilanValidasi(j, tanggal.ToDateTime(TimeOnly.MinValue))).ToList();
            if (invalid.Any())
            {
                Console.WriteLine($"Jenis berikut tidak dijadwalkan pada {tanggal.DayOfWeek}: {string.Join(", ", invalid)} \n");
                return;
            }

            try
            {
                jadwalService.CreateAndSendJadwal(tanggal, jenisList, namaKurir, area);
                Console.WriteLine("\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("\n");
            }
        }

        static void editFlow()
        {
            Console.WriteLine("\n∘<<──────>>∘ EDIT JADWAL PENGAMBILAN SAMPAH ∘<<──────>>∘ ");
            Console.Write("Masukkan nama kurir: ");
            var namaKurir = Console.ReadLine();
            Console.Write("Masukkan tanggal jadwal yang akan di-edit (yyyy-MM-dd): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var editDate))
            {
                Console.WriteLine("Format tanggal tidak valid.\n");
                return;
            }

            var jadwal = jadwalService.GetJadwalByKurirDanTanggal(namaKurir, editDate);
            if (jadwal == null)
            {
                Console.WriteLine("Jadwal tidak ditemukan di API.\n");
                return;
            }

            Console.WriteLine($"Data saat ini: Jenis [{string.Join(", ", jadwal.JenisSampah)}], Kurir [{jadwal.namaKurir}], Area [{jadwal.areaDiambil}]");

            var jenisValues = Enum.GetValues<JenisSampah>();
            Console.WriteLine("Pilih jenis sampah baru (pisahkan koma), atau enter untuk tetap:");
            for (int i = 0; i < jenisValues.Length; i++)
                Console.WriteLine($" {i}: {jenisValues[i]}");

            Console.Write("Pilih: ");
            var inputIdx = Console.ReadLine();
            var jenisList = string.IsNullOrWhiteSpace(inputIdx)
                ? jadwal.JenisSampah.Select(s => Enum.Parse<JenisSampah>(s)).ToList()
                : inputIdx.Split(',')
                          .Select(s => s.Trim())
                          .Where(s => int.TryParse(s, out _))
                          .Select(int.Parse)
                          .Where(i => i >= 0 && i < jenisValues.Length)
                          .Select(i => jenisValues[i])
                          .ToList();

            Console.Write($"Nama kurir baru (enter untuk [{jadwal.namaKurir}]): ");
            var newKurir = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newKurir)) newKurir = jadwal.namaKurir;

            Console.Write($"Area baru (enter untuk [{jadwal.areaDiambil}]): ");
            var area = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(area)) area = jadwal.areaDiambil;

            try
            {
                jadwalService.UpdateJadwal(editDate, jenisList, newKurir, area);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error update: {ex.Message}\n");
            }
        }

        static void DeleteFlow()
        {
            Console.Write("Masukkan nama kurir: ");
            var namaKurir = Console.ReadLine();
            Console.Write("Masukkan tanggal jadwal yang akan dihapus (yyyy-MM-dd): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var tanggal))
            {
                Console.WriteLine("Format tanggal tidak valid.");
                Console.WriteLine("\n");
                return;
            }

            jadwalService.DeleteJadwalByKurirDanTanggal(namaKurir, tanggal);
            Console.WriteLine("\n");
        }

    }
}
