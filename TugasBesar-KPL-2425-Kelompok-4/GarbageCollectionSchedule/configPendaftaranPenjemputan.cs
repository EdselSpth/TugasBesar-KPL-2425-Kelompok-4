using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using modelLibrary;
using JadwalAPI;
using TugasBesar_KPL_2425_Kelompok_4.UserProgram;

namespace TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule
{
    public class configPendaftaranPenjemputan<T>
    {
        private string configPath = "Riwayat_Pendaftaran_Penjemputan.json";
        public int id { get; set; }
        public string namaPengguna { get; set; }
        public configPendaftaraanArea Area { get; set; }
        public DateTime Jadwal { get; set; }
        public T Keterangan { get; set; }

        
        public configPendaftaranPenjemputan()
        {
            configPath = "Riwayat_Pendaftaran_Penjemputan.json";
        }

       
        public configPendaftaranPenjemputan(string path)
        {
            configPath = path;
        }
        public void Simpan()
        {
            List<configPendaftaranPenjemputan<T>> data = new();

            if (File.Exists(configPath))
            {
                string content = File.ReadAllText(configPath);


                if (!string.IsNullOrWhiteSpace(content))
                {
                    try
                    {
                        data = JsonSerializer.Deserialize<List<configPendaftaranPenjemputan<T>>>(content)
                            ?? new List<configPendaftaranPenjemputan<T>>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine("Gagal membaca file JSON: " + ex.Message);

                    }
                }
            }
            int maxId = 0;
            foreach (var item in data)
            {
                if (item.id > maxId)
                {
                    maxId = item.id;
                }
            }
            this.id = maxId + 1;
            data.Add(this);

            var serialized = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(configPath, serialized);
            Console.WriteLine("Pendaftaran berhasil disimpan.");

        }
     
        public List<configPendaftaranPenjemputan<T>> AmbilSemua()
        {
            if (!File.Exists(configPath)) return new List<configPendaftaranPenjemputan<T>>();

            string content = File.ReadAllText(configPath);
            if (string.IsNullOrWhiteSpace(content)) return new List<configPendaftaranPenjemputan<T>>();

            try
            {
                return JsonSerializer.Deserialize<List<configPendaftaranPenjemputan<T>>>(content)
                    ?? new List<configPendaftaranPenjemputan<T>>();
            }
            catch
            {
                return new List<configPendaftaranPenjemputan<T>>();
            }
        }
        public static void DaftarkanPengambilanSampah(string username)
        {
            configPendaftaraanArea areaConfig = new configPendaftaraanArea();
            List<configPendaftaraanArea> semuaArea = areaConfig.GetAllArea();
            
            if (semuaArea.Count == 0)
            {
                Console.WriteLine("Belum ada area yang tersedia. Silakan daftarkan area terlebih dahulu.");
                return;
            }

            Console.WriteLine("area pengambilan sampah");
            for (int i = 0; i < semuaArea.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {semuaArea[i].area}");
            }
            jadwalService.ViewJadwal();
            Console.Write("Masukkan nomor area terlebih dahulu: ");
            string input = Console.ReadLine();
            int nomorArea;

            if (!int.TryParse(input, out nomorArea))
            {
                Console.WriteLine("Input bukan angka.");
                return;
            }

            if (nomorArea < 1 || nomorArea > semuaArea.Count)
            {
                Console.WriteLine("Pilihan tidak valid.");
                return;
            }

           
            configPendaftaraanArea areaTerpilih = semuaArea[nomorArea - 1];
            
            Console.Write("Masukkan tanggal penjemputan (format: yyyy-MM-dd): ");
            string inputTanggal = Console.ReadLine();
            DateTime tanggalJemput;

            if (!DateTime.TryParse(inputTanggal, out tanggalJemput))
            {
                Console.WriteLine("Tanggal tidak valid.");
                return;
            }
            DateOnly tgl = DateOnly.FromDateTime(tanggalJemput);
            var hari = tgl.DayOfWeek;

            var jenisYangValid = Enum.GetValues(typeof(JenisSampah)).Cast<JenisSampah>().Where(js => rulesJadwal.pengambilanValidasi(js, tanggalJemput)).ToList();

            if (jenisYangValid.Count == 0)
            {
                Console.WriteLine("Tidak ada sampah yang dijadwalkan pada hari tersebut.");
                return;
            }

            Console.WriteLine("Sampah yang dapat disetorkan:");
            foreach (var jenis in jenisYangValid)
            {
                Console.WriteLine(jenis.ToString().ToLower());
            }
            Console.Write("Masukkan keterangan jumlah sampah: ");
            string keterangan = Console.ReadLine();

            var pendaftaran = new configPendaftaranPenjemputan<string>
            {
                namaPengguna = username,
                Area = areaTerpilih,
                Jadwal = tanggalJemput,
                Keterangan = keterangan
            };

            pendaftaran.Simpan();
        }


    }
}
