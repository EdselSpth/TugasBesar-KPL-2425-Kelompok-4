using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace modelLibrary
{
    public class pengguna
    {
        public string nama { get; set; }
        public JenisPengguna peran { get; set; }

        public pengguna(string namaInput, JenisPengguna peranInput)
        {
            Debug.Assert(namaInput != null, "Nama tidak boleh null");
            if(namaInput == null)
            {
                throw new ArgumentNullException(nameof(namaInput), "Nama Pengguna Tidak Boleh Kosong!!!");
            }
            nama = namaInput;
            peran = peranInput;
        }

        public pengguna()
        {
               
        }
    }
}
