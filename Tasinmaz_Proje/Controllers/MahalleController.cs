using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MahalleController : ControllerBase
    {
        private readonly IMahalleService _service;

        public MahalleController (IMahalleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mahalle>>> GetAllMahalleler()
        {
            var mahalleler = await _service.ListMahalle();
            return Ok(mahalleler);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mahalle>> GetMahalleById ( int id)
        {
            var mahalle = await _service.GetMahalleById(id);
            if (mahalle == null)
            {
                return NotFound();
            }
            return mahalle;
        }

        [HttpPost]
        public async Task<ActionResult<Mahalle>> AddMahalle( Mahalle mahalle)
        {
            await _service.AddMahalle(mahalle);
            return CreatedAtAction(nameof(GetMahalleById), new { id = mahalle.Id }, mahalle);
        }
    }
}
