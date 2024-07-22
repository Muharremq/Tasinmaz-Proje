using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Business.Abstract;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasinmazBilgiController : ControllerBase
    {
        private readonly ITasinmazBilgiService _tasinmazBilgiService;
        private readonly ILogService _logService;

        public TasinmazBilgiController(ITasinmazBilgiService tasinmazBilgiService, ILogService logService)
        {
            _tasinmazBilgiService = tasinmazBilgiService;
            _logService = logService;
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

            var log = new Log
            {
                KullaniciId = tasinmazBilgi.Id,
                Durum = "Başarılı",
                IslemTip = "Ekleme",
                Aciklama = $"Taşınmaz ID: {createdTasinmaz.Id} eklendi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };
            await _logService.AddLog(log);

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

            var log = new Log
            {
                KullaniciId = tasinmazBilgi.Id,
                Durum = "Başarılı",
                IslemTip = "Güncelleme",
                Aciklama = $"Taşınmaz ID: {updatedTasinmaz.Id} güncellendi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };
            await _logService.AddLog(log);

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

            var log = new Log
            {
                KullaniciId = id,
                Durum = "Başarılı",
                IslemTip = "Ekleme",
                Aciklama = $"Taşınmaz ID: {id} silindi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };
            await _logService.AddLog(log);
            return NoContent();
        }
    }
}
