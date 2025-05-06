using JadwalAPI.Model;

namespace JadwalAPI.Services
{
    public class JadwalService : IJadwalService
    {
        private readonly List<JadwalModel> _jadwalList = new()
        {
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now), JenisSampah = new List<string> { "Organik" }, namaKurir = "Andi", areaDiambil = "Ciganitri" },
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), JenisSampah = new List<string> { "Plastik" }, namaKurir = "Budi", areaDiambil = "Ciganitri"},
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), JenisSampah = new List<string> { "Kertas" }, namaKurir = "Joko", areaDiambil = "Ciganitri"},
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), JenisSampah = new List<string> { "Logam" }, namaKurir = "Oka", areaDiambil = "Ciganitri" },
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), JenisSampah = new List<string> { "Elektronik" }, namaKurir = "Eka", areaDiambil = "Ciganitri" },
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), JenisSampah = new List<string> { "BahanBerbahaya" }, namaKurir = "Herawan", areaDiambil = "Ciganitri" },
            new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(6)), JenisSampah = new List<string> { "Minyak" }, namaKurir = "Tono", areaDiambil = "Ciganitri" }
        };

        public List<JadwalModel> GetAll() => _jadwalList;

        public JadwalModel? GetByTanggal(DateOnly tanggal)
        {
            return _jadwalList.FirstOrDefault(j => j.Tanggal == tanggal);
        }

        public void TambahJadwal(JadwalModel jadwal)
        {
            _jadwalList.Add(jadwal);
        }

        public bool UpdateJadwal(DateOnly tanggal, JadwalModel updatedJadwal)
        {
            var existing = _jadwalList.FirstOrDefault(j => j.Tanggal == tanggal);
            if (existing == null) return false;

            existing.JenisSampah = updatedJadwal.JenisSampah;
            existing.namaKurir = updatedJadwal.namaKurir;
            existing.areaDiambil = updatedJadwal.areaDiambil;
            return true;
        }

        public bool HapusJadwal(DateOnly tanggal)
        {
            var existing = _jadwalList.FirstOrDefault(j => j.Tanggal == tanggal);
            if (existing == null) return false;

            _jadwalList.Remove(existing);
            return true;
        }
    }
}
