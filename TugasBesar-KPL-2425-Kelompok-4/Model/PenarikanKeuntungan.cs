using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugasBesar_KPL_2425_Kelompok_4.Model
{
    internal class PenarikanKeuntungan
    {
        public static bool VerifikasiRekening(string rekening)
        {
            return rekening.Length == 12;
        }

        private static Dictionary<JenisPengguna, Action> tindakanPenarikan = new Dictionary<JenisPengguna, Action>
        {
            { JenisPengguna.Pengguna, () => Console.WriteLine("Pengguna dapat memasukkan nomor rekening untuk mengambil keuntungan.") },
            { JenisPengguna.Admin, () => Console.WriteLine("Admin dapat menyetujui permintaan dan melakukan transfer ke nomor rekening.") }
            };

            public static void MelakukanPenarikan(JenisPengguna pengguna)
            {
                if (tindakanPenarikan.ContainsKey(pengguna))
                {
                    tindakanPenarikan[pengguna].Invoke();
                }
                else
                {
                    Console.WriteLine("Tindakan tidak tersedia untuk jenis pengguna ini.");
                }
            }
        }
    }
