using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace modelLibrary  
{
    public class Jadwal <T>
    {
        public DateOnly Tanggal { get; set; }
        public List<T> jenisSampahList { get; set; }
        public pengguna kurirPengambil { get; set; }
        public string areaDiambil { get; set; }

        public Jadwal(DateOnly tanggalInput, List<T> jenisSampahListInput, string areaDiambilInput, pengguna kurirPengambilInput)
        {
            Tanggal = tanggalInput;
            jenisSampahList = jenisSampahListInput;
            kurirPengambil = kurirPengambilInput;
            areaDiambil = areaDiambilInput;
        }

        public Jadwal(){ }
    }
}
