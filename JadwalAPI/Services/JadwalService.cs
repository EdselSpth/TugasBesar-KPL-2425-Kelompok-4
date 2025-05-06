using JadwalAPI.Model;
using JadwalAPI.Configuration;
using Microsoft.Extensions.Options;

namespace JadwalAPI.Services
{
    public class JadwalService : IJadwalService
    {
        private readonly JadwalSettings _settings;
        private readonly List<JadwalModel> _jadwalList;

        public JadwalService(IOptions<JadwalSettings> settings)
        {
            _settings = settings.Value;

            _jadwalList = new List<JadwalModel>
    {
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now), JenisSampah = new List<string> { "Organik" }, namaKurir = "Andi", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), JenisSampah = new List<string> { "Plastik" }, namaKurir = "Budi", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(1).ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), JenisSampah = new List<string> { "Kertas" }, namaKurir = "Joko", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(2).ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), JenisSampah = new List<string> { "Logam" }, namaKurir = "Oka", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(3).ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), JenisSampah = new List<string> { "Elektronik" }, namaKurir = "Eka", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(4).ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), JenisSampah = new List<string> { "BahanBerbahaya" }, namaKurir = "Herawan", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(5).ToString("dddd") },
        new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(6)), JenisSampah = new List<string> { "Minyak" }, namaKurir = "Tono", areaDiambil = _settings.DefaultArea, Hari = DateTime.Now.AddDays(6).ToString("dddd") }
    };
        }

        public List<JadwalModel> GetAll() => _jadwalList;

        public JadwalModel? GetByTanggal(DateOnly tanggal) =>
            _jadwalList.FirstOrDefault(j => j.Tanggal == tanggal);

        public void TambahJadwal(JadwalModel jadwal)
        {
            if (string.IsNullOrWhiteSpace(jadwal.areaDiambil))
                jadwal.areaDiambil = _settings.DefaultArea;

            _jadwalList.Add(jadwal);
        }

        public bool UpdateJadwal(DateOnly tanggal, JadwalModel updatedJadwal)
        {
            var existing = _jadwalList.FirstOrDefault(j => j.Tanggal == tanggal);
            if (existing == null) return false;

            existing.JenisSampah = updatedJadwal.JenisSampah;
            existing.namaKurir = updatedJadwal.namaKurir;
            existing.areaDiambil = updatedJadwal.areaDiambil ?? _settings.DefaultArea;
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
