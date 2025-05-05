using LoginAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User {Username = "admin", Password = "1234"}
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Username atau Password salah" });
            }

            return Ok(new { message = "Login berhasil!!", username = user.Username });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            var terdaftar = users.FirstOrDefault(u => u.Username == newUser.Username);

            if (terdaftar != null)
            {
                return Conflict(new { message = "Username sudah terdaftar" });
            }

            newUser.Id = users.Count + 1;
            users.Add(newUser);

            return Ok(new { message = "Register berhasil!!", username = newUser.Username });
        }
    }
}
