using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DurumController : ControllerBase
    {
        private readonly IDurumService _service;

        public DurumController (IDurumService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Durum>>> GetAllDurumlar()
        {
            var durumlar = await _service.ListDurum();
            return Ok(durumlar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Durum>> GetDurumById(int id)
        {
            var durum = await _service.GetDurumById(id);
            if (durum == null)
            {
                return NotFound();
            }
            return durum;
        }

        [HttpPost]
        public async Task<ActionResult<Durum>> AddDurum(Durum durum)
        {
            await _service.AddDurum(durum);
            return CreatedAtAction(nameof(GetDurumById), new { id = durum.Id }, durum);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDurum(int id, Durum durum)
        {
            if (id != durum.Id)
            {
                return BadRequest();
            }

            await _service.UpdateDurum(durum);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDurum(int id)
        {
            await _service.DeleteDurum(id);
            return NoContent();
        }
    }
}
