using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modelLibrary;
using JadwalAPI;
using JadwalAPI.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule
{
    public static class jadwalService
    {
        private static readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7277/") };

        /// <summary>
        /// Versi sync: Buat dan kirim jadwal sampah untuk beberapa jenis, serta simpan ke JSON lokal.
        /// </summary>
        public static void CreateAndSendJadwal(DateOnly tanggal, List<JenisSampah> jenisList, string namaKurir, string area)
        {
            if (jenisList == null || jenisList.Count == 0)
                throw new ArgumentException("Daftar jenis sampah tidak boleh kosong.", nameof(jenisList));

            var invalid = jenisList.Where(j => !rulesJadwal.pengambilanValidasi(j, tanggal.ToDateTime(TimeOnly.MinValue))).ToList();
            if (invalid.Any())
                throw new InvalidOperationException($"Jenis sampah berikut tidak dijadwalkan pada {tanggal.DayOfWeek}: {string.Join(", ", invalid)}.");

            var model = new JadwalModel
            {
                Tanggal = tanggal,
                JenisSampah = jenisList.Select(j => j.ToString()).ToList(),
                namaKurir = namaKurir ?? string.Empty,
                areaDiambil = area ?? string.Empty
            };

            var fileName = $"jadwal_{tanggal:yyyyMMdd}.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(fileName, JsonSerializer.Serialize(model, options));
            Console.WriteLine($"Tersimpan ke file {fileName}");

            var response = _http.PostAsJsonAsync("api/jadwal_admin", model).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Data berhasil dikirim ke API.");
        }

        public static void ViewJadwal()
        {
            var jadwalList = _http.GetFromJsonAsync<List<JadwalModel>>("api/jadwal_admin").Result;
            if (jadwalList == null || jadwalList.Count == 0)
            {
                Console.WriteLine("Tidak ada jadwal yang tersedia di API.");
                return;
            }
            Console.WriteLine("=== Daftar Jadwal ===");
            foreach (var j in jadwalList)
            {
                Console.WriteLine($"Tanggal: {j.Tanggal:yyyy-MM-dd} ({j.Hari})");
                Console.WriteLine($"  Jenis Sampah: {string.Join(", ", j.JenisSampah)}");
                Console.WriteLine($"  Kurir: {j.namaKurir}");
                Console.WriteLine($"  Area: {j.areaDiambil}");
                Console.WriteLine();
            }
        }

        public static JadwalModel GetJadwalByKurirDanTanggal(string namaKurir, DateOnly tanggal)
        {
            var response = _http.GetAsync("api/jadwal_admin").Result;
            if (!response.IsSuccessStatusCode) return null;
            var json = response.Content.ReadAsStringAsync().Result;
            var list = JsonSerializer.Deserialize<List<JadwalModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return list?.FirstOrDefault(j => j.namaKurir.Equals(namaKurir, StringComparison.OrdinalIgnoreCase) && j.Tanggal == tanggal);
        }

        public static void UpdateJadwalSync(DateOnly tanggal, List<JenisSampah> jenisList, string namaKurir, string area)
        {
            var model = new JadwalModel
            {
                Tanggal = tanggal,
                JenisSampah = jenisList.Select(j => j.ToString()).ToList(),
                namaKurir = namaKurir,
                areaDiambil = area
            };

            var response = _http.PutAsJsonAsync($"api/jadwal_admin/{tanggal:yyyy-MM-dd}", model).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Data berhasil diupdate ke API.");

            var fileName = $"jadwal_{tanggal:yyyyMMdd}.json";
            File.WriteAllText(fileName, JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine($"File lokal {fileName} berhasil diperbarui.");
        }

        public static void DeleteJadwalByKurirDanTanggal(string namaKurir, DateOnly tanggal)
        {
            var jadwal = GetJadwalByKurirDanTanggal(namaKurir, tanggal);
            if (jadwal == null)
            {
                Console.WriteLine("Jadwal tidak ditemukan.");
                return;
            }

            var response = _http.DeleteAsync($"api/jadwal_admin/{tanggal:yyyy-MM-dd}").Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Jadwal berhasil dihapus dari API.");
                var fileName = $"jadwal_{tanggal:yyyyMMdd}.json";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Console.WriteLine($"File lokal {fileName} juga dihapus.");
                }
            }
            else
            {
                Console.WriteLine("Gagal menghapus jadwal dari API.");
            }
        }
    }
}
