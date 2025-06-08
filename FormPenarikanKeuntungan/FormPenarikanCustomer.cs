using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static TugasBesar_KPL_2425_Kelompok_4.Penarikan_Keuntungan.StateBasedPenarikan;

namespace FormPenarikanKeuntungan
{
    public partial class FormPenarikanCustomer : Form
    {
        private string nominal;
        private string rekening;
        private string metodePembayaran;

        public FormPenarikanCustomer()
        {
            InitializeComponent();
            Load += FormPenarikanCustomer_Load;
            button1.Click += button1_Click;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void FormPenarikanCustomer_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[]
            {"Tunai", "Bca", "Bni", "Mandiri", "Bri", "ShopeePay", "Gopay", "Dana"});

            button1.Text = "Kirim";
            this.Text = "Form Penarikan Customer";
        }

        //untuk menginput nominal
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            nominal = textBox1.Text;
            UpdateTotalDiterimaLabel();
        }

        //untuk menginput rekening
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string input = textBox2.Text;

            string angkaSaja = new string(input.Where(char.IsDigit).ToArray());

            if (angkaSaja.Length > 12)
                angkaSaja = angkaSaja.Substring(0, 12);

            textBox2.Text = angkaSaja;
            textBox2.SelectionStart = angkaSaja.Length;
            rekening = angkaSaja;
        }

        //untuk memilih metode pembayaran
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            metodePembayaran = comboBox1.SelectedItem?.ToString();
            UpdateTotalDiterimaLabel();
        }

        //untuk mengirim data penarikan
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nominal) || string.IsNullOrWhiteSpace(rekening) || string.IsNullOrWhiteSpace(metodePembayaran))
            {
                MessageBox.Show("Harap lengkapi semua data penarikan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(nominal, out decimal nominalDecimal))
            {
                MessageBox.Show("Nominal tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Enum.TryParse(metodePembayaran, true, out Pembayaran metode))
            {
                MessageBox.Show("Metode pembayaran tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var info = PenarikanCustomer.PembayaranTable[metode];

            if (nominalDecimal < info.MinimalPenarikan)
            {
                MessageBox.Show($"Minimal penarikan untuk metode ini adalah {info.MinimalPenarikan}.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalDiterima = nominalDecimal - info.BiayaAdmin;

            PenarikanCustomer.RiwayatPenarikan.Add(new PenarikanData(rekening, nominalDecimal, metode));

            PenarikanState currentState = PenarikanState.MEMASUKKAN_DATA;
            currentState = StateBasedPenarikan.GetNextState(currentState, PenarikanTrigger.SUBMIT);

            MessageBox.Show("Memproses penarikan, mohon tunggu...", "Proses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await System.Threading.Tasks.Task.Delay(2000);
            currentState = PenarikanState.BERHASIL;
            MessageBox.Show(
                $"Penarikan berhasil!\n" +
                $"Nomor Rekening: {rekening}\n" +
                $"Metode Pembayaran: {metodePembayaran}\n" +
                $"Total diterima setelah admin ({info.BiayaAdmin}): {totalDiterima}\n" +
                $"Status: {currentState}",
                "Sukses",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        //hanya fungsi yang berguna untuk memperbarui label total diterima
        private void UpdateTotalDiterimaLabel()
        {
            if (decimal.TryParse(nominal, out decimal nominalDecimal) &&
                Enum.TryParse(metodePembayaran, true, out Pembayaran metode) &&
                PenarikanCustomer.PembayaranTable.TryGetValue(metode, out var info))
            {
                decimal totalDiterima = nominalDecimal - info.BiayaAdmin;

                // Pastikan tidak tampil negatif
                if (totalDiterima >= 0)
                    label5.Text = $"Total Diterima: {totalDiterima}";
                else
                    label5.Text = $"Total Diterima: 0";
            }
            else
            {
                label5.Text = "Total Diterima: -";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
