using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Business.Abstract;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasinmazBilgiController : ControllerBase
    {
        private readonly ITasinmazBilgiService _tasinmazBilgiService;

        public TasinmazBilgiController(ITasinmazBilgiService tasinmazBilgiService)
        {
            _tasinmazBilgiService = tasinmazBilgiService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasinmazBilgi>>> GetAllTasinmazlar()
        {
            var tasinmazlar = await _tasinmazBilgiService.GetAllTasinmazBilgi();
            return Ok(tasinmazlar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TasinmazBilgi>> GetTasinmazById(int id)
        {
            var tasinmaz = await _tasinmazBilgiService.GetTasinmazBilgiById(id);
            if (tasinmaz == null)
            {
                return NotFound();
            }
            return Ok(tasinmaz);
        }

        [HttpPost]
        public async Task<ActionResult<TasinmazBilgi>> AddTasinmaz(TasinmazBilgi tasinmazBilgi)
        {
            var createdTasinmaz = await _tasinmazBilgiService.AddTasinmaz(tasinmazBilgi);
            return CreatedAtAction(nameof(GetTasinmazById), new { id = createdTasinmaz.Id }, createdTasinmaz);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TasinmazBilgi>> UpdateTasinmaz(int id, TasinmazBilgi tasinmazBilgi)
        {
            if (id != tasinmazBilgi.Id)
            {
                return BadRequest();
            }

            var updatedTasinmaz = await _tasinmazBilgiService.UpdateTasinmaz(tasinmazBilgi);
            return Ok(updatedTasinmaz);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTasinmaz(int id)
        {
        var result = await _tasinmazBilgiService.DeleteTasinmaz(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
        }
    }
}
