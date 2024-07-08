using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IlceController : ControllerBase
    {
        private readonly IIlceService _service;

        public IlceController (IIlceService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ilce>>> GetAllIlceler()
        {
            var ilceler = await _service.ListIlce();
            return Ok(ilceler);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Ilce>> GetIlceById(int id)
        {
            var ilce = await _service.GetIlceById(id);
            if (ilce == null)
            {
                return NotFound();
            }
            return ilce;
        }
        [HttpPost]
        public async Task<ActionResult<Ilce>> AddIlce(Ilce ilce)
        {
            await _service.AddIlce(ilce);
            return CreatedAtAction(nameof(GetIlceById), new {id = ilce.Id}, ilce);
        }
    }
}
