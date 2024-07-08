using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IslemTipController : ControllerBase
    {
        private readonly IIslemTipService _service;

        public IslemTipController (IIslemTipService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IslemTip>>> GetAllIslemTipler()
        {
            var islemTipler = await _service.ListIslemTip();
            return Ok(islemTipler);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IslemTip>> GetIslemTipById(int id)
        {
            var islemTip = await _service.GetIslemTipById(id);
            if (islemTip == null)
            {
                return NotFound();
            }
            return islemTip;
        }

        [HttpPost]
        public async Task<ActionResult<IslemTip>> AddIslemTip(IslemTip islemTip)
        {
            await _service.AddIslemTip(islemTip);
            return CreatedAtAction(nameof(GetIslemTipById), new { id = islemTip.Id }, islemTip);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIslemTip(int id,  IslemTip islemTip)
        {
            if (id != islemTip.Id)
            {
                return BadRequest();
            }
            await _service.UpdateIslemTip(islemTip);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIslemTip(int id)
        {
            await _service.DeleteIslemTip(id);
            return NoContent();
        }
    }
}
