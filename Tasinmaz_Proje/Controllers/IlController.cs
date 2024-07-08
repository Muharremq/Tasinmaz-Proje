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
    }
}
