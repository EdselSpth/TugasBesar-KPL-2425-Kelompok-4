using Microsoft.AspNetCore.Mvc;
using JadwalAPI.Model;
using JadwalAPI.Services;

namespace JadwalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Jadwal_Kurir : ControllerBase
    {
        private readonly IJadwalService _jadwalService;

        public Jadwal_Kurir(IJadwalService jadwalService)
        {
            _jadwalService = jadwalService;
        }

        [HttpGet]
        public ActionResult<List<JadwalModel>> GetAll()
        {
            return Ok(_jadwalService.GetAll());
        }

        [HttpGet("{tanggal}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<List<JadwalModel>> GetByTanggal(string tanggal)
        {
            if (!DateOnly.TryParse(tanggal, out DateOnly parsedDate))
                return BadRequest("Format tanggal tidak valid. Gunakan format yyyy-MM-dd.");

            var jadwal = _jadwalService.GetByTanggal(parsedDate);
            if (jadwal == null)
                return NotFound("Jadwal tidak ditemukan.");

            return Ok(jadwal);
        }
    }
}
