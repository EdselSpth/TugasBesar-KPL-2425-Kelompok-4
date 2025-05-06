using JadwalAPI.Model;
using TugasBesar_KPL_2425_Kelompok_4.Model;

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
