using JadwalAPI.Model;
using modelLibrary;

namespace JadwalAPI.Services
{
    public interface IJadwalService
    {
        List<JadwalModel> GetAll();
        JadwalModel? GetByTanggal(DateOnly tanggal);
        void TambahJadwal(JadwalModel jadwal);
        bool UpdateJadwal(DateOnly tanggal, JadwalModel updatedJadwal);
        bool HapusJadwal(DateOnly tanggal);
    }
}
