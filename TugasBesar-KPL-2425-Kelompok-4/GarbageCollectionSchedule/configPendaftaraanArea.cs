using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TugasBesar_KPL_2425_Kelompok_4.GarbageCollectionSchedule
{

    public class configPendaftaraanArea
    {
        private string configPath = "daftarArea.json";
        public int id { get; set; }
        public string area { get; set; }
        // Constructor dengan default path (untuk produksi)
        public configPendaftaraanArea() : this("daftarArea.json") { }

        // Constructor dengan custom path (untuk unit test)
        public configPendaftaraanArea(string customPath)
        {
            configPath = customPath;
        }
        public List<configPendaftaraanArea> GetAllArea()
        {
            List<configPendaftaraanArea> listArea = new();
            if (File.Exists(configPath))
            {
                string read = File.ReadAllText(configPath);
                if (!string.IsNullOrWhiteSpace(read))
                {
                    listArea = JsonSerializer.Deserialize<List<configPendaftaraanArea>>(read);
                    if (listArea == null)
                    {
                        listArea = new List<configPendaftaraanArea>();
                    }
                }
            }
            return listArea;
        }

        public void saveArea()
        {
            try
            {
                var listArea = GetAllArea();
                // Cegah area duplikat
                if (listArea.Any(a => a.area != null &&
                                      a.area.Equals(this.area, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Area sudah ada. Tidak disimpan ulang.");
                    return;
                }
                int maxId = 0;
                foreach (var i in listArea)
                {
                    if (i.id > maxId)
                    {
                        maxId = i.id;
                    }
                }

                this.id = maxId + 1;
                listArea.Add(this);

                string newData = JsonSerializer.Serialize(listArea, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, newData);
                Console.WriteLine(" Data Area berhasil disimpan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan saat menyimpan data area.");
                Console.WriteLine($"Detail: {ex.Message}");
            }
        }
        public void TampilkanSemuaArea()
        {
            var listArea = GetAllArea();

            if (listArea.Count == 0)
            {
                Console.WriteLine("Belum ada area yang terdaftar.");
                return;
            }

            Console.WriteLine("Daftar Area Pengambilan Sampah:");
            foreach (var area in listArea)
            {
                Console.WriteLine($"ID: {area.id} | Nama Area: {area.area}");
            }
        }
        public static void DaftarkanAreaPengambilan()
        {
            Console.Write("Masukkan nama area baru: ");
            string namaAreaBaru = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(namaAreaBaru))
            {
                Console.WriteLine("Nama area tidak boleh kosong.");
                return;
            }

            configPendaftaraanArea areaConfig = new configPendaftaraanArea();
            List<configPendaftaraanArea> daftarArea = areaConfig.GetAllArea();

            bool duplikat = false;

            foreach (var area in daftarArea)
            {
                if (area.area != null && area.area.Equals(namaAreaBaru, StringComparison.OrdinalIgnoreCase))
                {
                    duplikat = true;
                    break;
                }
            }

            if (duplikat)
            {
                Console.WriteLine("Area sudah terdaftar. Silakan masukkan nama area yang berbeda.");
            }
            else
            {
                configPendaftaraanArea areaBaru = new configPendaftaraanArea
                {
                    area = namaAreaBaru
                };
                areaBaru.saveArea();
            }
        }

    }
};