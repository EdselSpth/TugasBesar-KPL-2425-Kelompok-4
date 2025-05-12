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
        string configPath = "daftarArea.json";
        public int id { get; set; }
        public string area { get; set; }

        List<configPendaftaraanArea> listArea = new List<configPendaftaraanArea>();
        public void LoadConvig()
        {
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
        }
        public void saveArea()
        {
            try
            {
                LoadConvig();

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
                Console.WriteLine("Data area berhasil disimpan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan saat menyimpan data area.");
                Console.WriteLine($"Detail: {ex.Message}");
            }
        }

    };

            
}
