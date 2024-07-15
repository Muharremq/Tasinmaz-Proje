using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IlController : ControllerBase
    {
        private readonly IIlService _service;

        public IlController(IIlService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Il>>> GetAllIl()
        {
            var iller = await _service.ListIl();
            return Ok(iller);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Il>> GetIlById (int id)
        {
            var il = await _service.GetIlById(id);
            if (il == null)
            {
                return NotFound();
            }
            return il;
        }

        [HttpPost]
        public async Task<ActionResult<Il>> AddIl(Il il)
        {
            await _service.AddIl(il);
            return CreatedAtAction(nameof(GetIlById), new {id = il.Id}, il);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIl(int id)
        {
            await _service.DeleteIl(id);
            return NoContent();
        }


        [HttpPost("import/json")]
        public async Task<IActionResult> ImportIllerFromJson()
        {
            var filePath = @"C:\Users\user\Desktop\4821a26db048cc0972c1beee48a408de-4754e5f9d09dade2e6c461d7e960e13ef38eaa88\cityList.json"; // Dosya yolunu buraya girin
            await _service.AddIllerFromJsonFileAsync(filePath);
            return Ok();
        }
    }
}
