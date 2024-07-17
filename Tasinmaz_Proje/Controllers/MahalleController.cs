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

        [HttpPost("import-neighborhoods")]
        public IActionResult ImportNeighborhoodsFromJson()
        {
            var filePath = @"C:\Users\user\Desktop\4821a26db048cc0972c1beee48a408de-4754e5f9d09dade2e6c461d7e960e13ef38eaa88\neighbourhoodsByDistrictAndCityCode.json"; // Dosya yolunu buraya girin

            try
            {
                _service.AddNeighborhoodsFromJsonFileAsync(filePath).Wait(); // Wait for completion, can be improved with async await

                return Ok("Neighborhoods imported successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error importing neighborhoods: {ex.Message}");
            }
        }

        [HttpGet("by-ilce/{ilceId}")]
        public async Task<ActionResult<IEnumerable<Mahalle>>> GetMahallelerByIlceId (int ilceId)
        {
            var mahalleler = await _service.GetMahallelerByIlceId(ilceId);
            if(mahalleler == null || mahalleler.Count == 0)
            {
                return NotFound();
            }
            return Ok(mahalleler);
        }
    }
}
