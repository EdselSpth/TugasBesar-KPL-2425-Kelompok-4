using LoginAPI.Models;
using System.Net.Http.Json;
using TugasBesar_KPL_2425_Kelompok_4.UserProgram;

namespace TugasBesar_KPL_2425_Kelompok_4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Dispaly Header
            await header();
            // Display Menu
            await ShowMainMenu();
        }

        static async Task header()
        {
            Console.WriteLine("ReGreen");
            Console.WriteLine("Kelompok 4");
            Console.WriteLine("Edsel Septa Haryanto - 103022300016");
            Console.WriteLine("Gusti Agung Raka Darma Putra Kepakisan - 103022300088");
            Console.WriteLine("Deru Khairan Djuharianto - 103022300101");
            Console.WriteLine("Reza Indra Maulana - 103022300109");
            Console.WriteLine("Abdul Azis Saepurohmat - 103022300092");
            Console.WriteLine("Tio Funny Tinambunan - 103022330036");
            Console.WriteLine("=====================================\n");
        }

        static async Task ShowMainMenu()
        {
            Console.WriteLine("Pilih opsi:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Pilihan Anda: ");
            string pilihan = Console.ReadLine();

            if (pilihan == "1")
            {
                await LoginAsync();
            }
            else if (pilihan == "2")
            {
                await RegisterAsync();
            }
            else if (pilihan == "3")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid.");
                await ShowMainMenu();  // Menampilkan menu lagi jika input tidak valid
            }
        }

        static async Task LoginAsync()
        {
            Console.Clear();
            Console.WriteLine("\n=== Login ===");
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string pass = Console.ReadLine();

            var loginUser = new User
            {
                Username = username,
                Password = pass
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5296");

            var response = await client.PostAsJsonAsync("/api/auth/login", loginUser);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                Console.WriteLine($"\nLogin berhasil sebagai {result.Username} (Role: {result.Role})");

                switch (result.Role)
                {
                    case Role.Admin:
                        Admin.adminProgram();
                        break;
                    case Role.Kurir:
                        Kurir.KurirProgram();
                        break;
                    case Role.User:
                        Pengguna.PenggunaProgram();
                        break;
                    default:
                        Console.WriteLine("Role tidak dikenali.");
                        break;
                }

                // Kembali ke menu utama setelah selesai
                await ShowMainMenu();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login Gagal! {error}");
                await ShowMainMenu();  // Kembali ke menu utama jika login gagal
            }
        }

        static async Task RegisterAsync()
        {
            Console.Clear();
            Console.WriteLine("\n=== Register ===");
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Role (Admin/Kurir/User): ");
            string roleInput = Console.ReadLine();
            Role role = Enum.TryParse(roleInput, true, out role) ? role : Role.User;

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5296");

            var response = await client.PostAsJsonAsync("/api/auth/register", newUser);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Register berhasil!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Register gagal! {error}");
            }

            // Kembali ke menu utama setelah registrasi selesai
            await ShowMainMenu();
        }
    }

    public class LoginResult
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}
