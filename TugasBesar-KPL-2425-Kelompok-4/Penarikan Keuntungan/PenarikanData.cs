using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan
{
    public class PenarikanData
    {
        public string NomorRekening { get; }
        public decimal Nominal { get; }
        public Pembayaran MetodePembayaran { get; }

        public PenarikanData(string norek, decimal nominal, Pembayaran metode)
        {
            NomorRekening = norek;
            Nominal = nominal;
            MetodePembayaran = metode;
        }
    }
}
