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
    }
}
