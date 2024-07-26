using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Business.Abstract;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasinmazBilgiController : ControllerBase
    {
        private readonly ITasinmazBilgiService _tasinmazBilgiService;
        private readonly IAuthRepository _authRepository;
        private readonly ILogService _logService;

        public TasinmazBilgiController(ITasinmazBilgiService tasinmazBilgiService, IAuthRepository authRepository, ILogService logService)
        {
            _tasinmazBilgiService = tasinmazBilgiService;
            _authRepository = authRepository;
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]

        public async Task<ActionResult<IEnumerable<TasinmazBilgi>>> GetAllTasinmazlar()
        {


            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var isAdmin = await _authRepository.IsAdmin(userEmail);
                if (isAdmin)
                {
                    var tasinmazlar = await _tasinmazBilgiService.GetAllTasinmazBilgi();
                    return Ok(tasinmazlar);
                }else
                {
                    var tasinmazlar = await _tasinmazBilgiService.GetTasinmazlarByUserId(userId);
                    return Ok(tasinmazlar);
                }
            }catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]

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
        [Authorize(Roles = "admin,user")]

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
        [Authorize(Roles = "admin,user")]

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
        [Authorize(Roles = "admin,user")]

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

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "admin,user")]

        public async Task<ActionResult<IEnumerable<TasinmazBilgi>>> GetTasinmazlarByUserId(int userId)
        {
            var tasinmazlar = await _tasinmazBilgiService.GetTasinmazlarByUserId(userId);
            if (tasinmazlar == null || !tasinmazlar.Any())
            {
                return NotFound();
            }
            return Ok(tasinmazlar);
        }
    }
}
