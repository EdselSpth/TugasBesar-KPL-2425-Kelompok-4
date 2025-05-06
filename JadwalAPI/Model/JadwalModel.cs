using System;
using System.Collections.Generic;
using TugasBesar_KPL_2425_Kelompok_4.Model;

namespace JadwalAPI.Model
{
    public class JadwalModel
    {
        public DateOnly Tanggal { get; set; }

        // Change this to a List of strings for serialization
        public List<string> JenisSampah { get; set; } = new();

        public string namaKurir { get; set; } = string.Empty;
        public string areaDiambil { get; set; } = string.Empty;

        // You can keep this for the formatted weekday string
        public string Hari => Tanggal.DayOfWeek.ToString();
    }
}
