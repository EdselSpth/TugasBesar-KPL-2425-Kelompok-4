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
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Globalization;
using System.Text.Json.Serialization;

namespace TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule
{
    public static class jadwalService
    {
        private static readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://localhost:7277/") };

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
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            List<JadwalModel> semuaJadwal;

            if (File.Exists(fileName))
            {
                var existingJson = File.ReadAllText(fileName).Trim();
                if (existingJson.StartsWith("["))
                {
                    semuaJadwal = JsonSerializer.Deserialize<List<JadwalModel>>(existingJson, options)
                                  ?? new List<JadwalModel>();
                }
                else if (existingJson.StartsWith("{"))
                {
                    var single = JsonSerializer.Deserialize<JadwalModel>(existingJson, options);
                    semuaJadwal = single != null
                        ? new List<JadwalModel> { single }
                        : new List<JadwalModel>();
                }
                else
                {
                    // File kosong atau format aneh
                    semuaJadwal = new List<JadwalModel>();
                }
            }
            else
            {
                semuaJadwal = new List<JadwalModel>();
            }

            // Tambahkan model baru
            semuaJadwal.Add(model);

            // Serialisasi ulang sebagai array
            var arrayJson = JsonSerializer.Serialize(semuaJadwal, options);
            File.WriteAllText(fileName, arrayJson);
            Console.WriteLine($"Tersimpan ke file {fileName} (total {semuaJadwal.Count} entri)");

            // Kirim hanya object baru ke API
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

        public static void UpdateJadwal(DateOnly tanggal, List<JenisSampah> jenisList, string namaKurir, string area)
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
            // 1. Panggil API
            var apiResponse = _http.DeleteAsync($"api/jadwal_admin/{tanggal:yyyy-MM-dd}").Result;
            if (!apiResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Gagal menghapus jadwal dari API.");
                return;
            }
            Console.WriteLine("Jadwal berhasil dihapus dari API.");

            // 2. Hapus entri di file lokal
            var fileName = $"jadwal_{tanggal:yyyyMMdd}.json";
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} tidak ditemukan, tidak ada yang perlu dihapus.");
                return;
            }

            // Siapkan serializer dengan converter DateOnly
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new DateOnlyJsonConverter() }
            };

            // Baca dan deserialize seluruh array
            var json = File.ReadAllText(fileName);
            var list = JsonSerializer.Deserialize<List<JadwalModel>>(json, options)
                            ?? new List<JadwalModel>();

            // Cari entri matching dan hapus
            var removed = list.RemoveAll(j =>
                j.namaKurir.Equals(namaKurir, StringComparison.OrdinalIgnoreCase)
                && j.Tanggal == tanggal);

            if (removed == 0)
            {
                Console.WriteLine("Tidak ada entri jadwal di file yang cocok untuk dihapus.");
                return;
            }

            // Jika setelah hapus list kosong → delete file; kalau masih ada → tulis ulang
            if (list.Count == 0)
            {
                File.Delete(fileName);
                Console.WriteLine($"Semua entri di file {fileName} dihapus; file dihapus.");
            }
            else
            {
                var updatedJson = JsonSerializer.Serialize(list, options);
                File.WriteAllText(fileName, updatedJson);
                Console.WriteLine($"Entitas jadwal '{namaKurir}' pada {tanggal:yyyy-MM-dd} dihapus dari {fileName}.");
            }
        }
    }
}
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var s = reader.GetString() ?? throw new JsonException("Expected date string");
        return DateOnly.ParseExact(s, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}