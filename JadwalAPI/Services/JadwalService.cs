using JadwalAPI.Model;
using JadwalAPI.Configuration;
using Microsoft.Extensions.Options;
using modelLibrary;

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
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now), JenisSampah = new List<string> { JenisSampah.Organik.ToString() }, namaKurir = "Andi", areaDiambil = "Buahbatu" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), JenisSampah = new List<string> { JenisSampah.Plastik.ToString() }, namaKurir = "Budi", areaDiambil = "Bojongsoang" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), JenisSampah = new List<string> { JenisSampah.Kertas.ToString() }, namaKurir = "Joko", areaDiambil = "Cimahi" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), JenisSampah = new List<string> { JenisSampah.Logam.ToString() }, namaKurir = "Oka", areaDiambil = "Pasirkoja" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), JenisSampah = new List<string> { JenisSampah.Elektronik.ToString() }, namaKurir = "Eka", areaDiambil = "Gedebage" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), JenisSampah = new List<string> { JenisSampah.BahanBerbahaya.ToString() }, namaKurir = "Herawan", areaDiambil = "Kiaracondong" },
                new JadwalModel { Tanggal = DateOnly.FromDateTime(DateTime.Now.AddDays(6)), JenisSampah = new List<string> { JenisSampah.Minyak.ToString() }, namaKurir = "Tono", areaDiambil = "Ciganitri" }
            };
        }

        public List<JadwalModel> GetAll() => _jadwalList;

        public List<JadwalModel> GetByTanggal(DateOnly tanggal) =>
    _jadwalList.Where(j => j.Tanggal == tanggal).ToList();

        public void TambahJadwal(JadwalModel jadwal)
        {
            if (string.IsNullOrWhiteSpace(jadwal.areaDiambil))
                jadwal.areaDiambil = _settings.DefaultArea;

            // Ensure JenisSampah is being passed as a List<string> for consistency
            if (jadwal.JenisSampah != null && jadwal.JenisSampah.Count == 0)
            {
                jadwal.JenisSampah = new List<string> { JenisSampah.Organik.ToString() }; // Default if empty
            }

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
