using LoginAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "1234", Role = Role.Admin }
        };

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var state = LoginState.Awal;
            state = StateBasedAuth.GetNextState(state, LoginTrigger.Submit);
            state = StateBasedAuth.GetNextState(state,
                users.Any(u => u.Username == loginUser.Username && u.Password == loginUser.Password)
                ? LoginTrigger.Valid
                : LoginTrigger.Invalid);

            if (state == LoginState.Berhasil)
            {
                var user = users.First(u => u.Username == loginUser.Username);
                return Ok(new { message = "Login berhasil!", username = user.Username, role = user.Role });
            }
            else
            {
                return Unauthorized(new { message = "Username atau password salah" });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            var existingUser = users.FirstOrDefault(u => u.Username == newUser.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username sudah terdaftar" });
            }

            newUser.Id = users.Count + 1;
            users.Add(newUser);

            return Ok(new { message = "Register berhasil!", username = newUser.Username, role = newUser.Role });
        }
    }
}
