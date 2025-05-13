using Microsoft.AspNetCore.Mvc;
using JadwalAPI.Model;
using JadwalAPI.Services;

namespace JadwalAPI.Controllers
{
    [ApiController]
    [Route("api/jadwal_admin")]
    public class JadwalAdminController : ControllerBase
    {
        private readonly IJadwalService _jadwalService;

        public JadwalAdminController(IJadwalService jadwalService)
        {
            _jadwalService = jadwalService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<JadwalModel>), 200)]
        public ActionResult<List<JadwalModel>> GetAll()
        {
            return Ok(_jadwalService.GetAll());
        }

        [HttpGet("{tanggal}")]
        [ProducesResponseType(typeof(List<JadwalModel>), 200)]
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

        [HttpPost]
        [ProducesResponseType(typeof(JadwalModel), 201)]
        [ProducesResponseType(400)]
        public ActionResult AddJadwal([FromBody] JadwalModel jadwal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _jadwalService.TambahJadwal(jadwal);
            return CreatedAtAction(nameof(GetByTanggal), new { tanggal = jadwal.Tanggal.ToString("yyyy-MM-dd") }, jadwal);
        }

        [HttpPut("{tanggal}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateJadwal(string tanggal, [FromBody] JadwalModel updatedJadwal)
        {
            if (!DateOnly.TryParse(tanggal, out DateOnly parsedDate))
                return BadRequest("Format tanggal tidak valid. Gunakan format yyyy-MM-dd.");

            bool success = _jadwalService.UpdateJadwal(parsedDate, updatedJadwal);
            if (!success)
                return NotFound("Jadwal tidak ditemukan.");

            return NoContent();
        }

        [HttpDelete("{tanggal}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteJadwal(string tanggal)
        {
            if (!DateOnly.TryParse(tanggal, out DateOnly parsedDate))
                return BadRequest("Format tanggal tidak valid. Gunakan format yyyy-MM-dd.");

            bool success = _jadwalService.HapusJadwal(parsedDate);
            if (!success)
                return NotFound("Jadwal tidak ditemukan.");

            return NoContent();
        }
    }
}
