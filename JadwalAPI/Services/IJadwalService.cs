using JadwalAPI.Model;
using modelLibrary;

namespace JadwalAPI.Services
{
    public interface IJadwalService
    {
        List<JadwalModel> GetAll();
        List<JadwalModel> GetByTanggal(DateOnly tanggal); // sudah sesuai: mengembalikan semua yang cocok
        void TambahJadwal(JadwalModel jadwal);
        bool UpdateJadwal(DateOnly tanggal, JadwalModel updatedJadwal);
        bool HapusJadwal(DateOnly tanggal);
    }
}
