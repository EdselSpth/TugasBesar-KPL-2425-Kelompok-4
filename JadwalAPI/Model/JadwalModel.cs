namespace JadwalAPI.Model
{
    public class JadwalModel
    {
        public DateOnly Tanggal { get; set; }
        public List<string> JenisSampah { get; set; }
        public string namaKurir { get; set; }
        public string areaDiambil { get; set; }
    }
}
