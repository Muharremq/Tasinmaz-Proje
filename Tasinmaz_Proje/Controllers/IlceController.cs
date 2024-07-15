using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpPost("import-districts")]
        public IActionResult ImportDistrictsFromJson()
        {
            var filePath = @"C:\Users\user\Desktop\4821a26db048cc0972c1beee48a408de-4754e5f9d09dade2e6c461d7e960e13ef38eaa88\districtsByCityCode.json"; // Dosya yolunu buraya girin

            try
            {
                _service.AddDistrictsFromJsonFileAsync(filePath).Wait(); // Wait for completion, can be improved with async await

                return Ok("Districts imported successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error importing districts: {ex.Message}");
            }
        }

        [HttpGet("by-city/{cityId}")]
        public async Task<ActionResult<IEnumerable<Ilce>>> GetIlcelerByIlId(int cityId)
        {
            var ilceler = await _service.GetIlcelerByIlId(cityId);
            if (ilceler == null || ilceler.Count == 0)
            {
                return NotFound();
            }
            return Ok(ilceler);
        }
    }
}
