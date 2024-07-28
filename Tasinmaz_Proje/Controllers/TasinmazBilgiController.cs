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

        private async Task<string> GetUserRoleFromToken()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var isAdmin = await _authRepository.IsAdmin(userEmail);
            return isAdmin ? "Admin" : "User";
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value);
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<IEnumerable<TasinmazBilgi>>> GetAllTasinmazlar()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var userRole = await GetUserRoleFromToken();

                IEnumerable<TasinmazBilgi> tasinmazlar;

                if (userRole == "Admin")
                {
                    tasinmazlar = await _tasinmazBilgiService.GetAllTasinmazBilgi();
                }
                else
                {
                    tasinmazlar = await _tasinmazBilgiService.GetTasinmazlarByUserId(userId);
                }

                return Ok(tasinmazlar);
            }
            catch (Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = GetUserIdFromToken(),
                    Durum = "Başarısız",
                    IslemTip = "Taşınmazları Getirme",
                    Aciklama = $"Taşınmazları getirirken hata oluştu: {ex.Message}",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                Console.WriteLine($"Error getting Tasinmazlar: {ex.Message}");
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
            try
            {
                var createdTasinmaz = await _tasinmazBilgiService.AddTasinmaz(tasinmazBilgi);

                var log = new Log
                {
                    KullaniciId = tasinmazBilgi.UserId,
                    Durum = "Başarılı",
                    IslemTip = "Ekleme",
                    Aciklama = $"Taşınmaz ID: {createdTasinmaz.Id} eklendi",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };

                await _logService.AddLog(log);
                return CreatedAtAction(nameof(GetTasinmazById), new { id = createdTasinmaz.Id }, createdTasinmaz);
            }
            catch (Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = tasinmazBilgi.UserId,
                    Durum = "Başarısız",
                    IslemTip = "Ekleme",
                    Aciklama = $"Taşınmaz eklenirken hata oluştu.",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                Console.WriteLine($"Error adding Tasinmaz: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult<TasinmazBilgi>> UpdateTasinmaz(int id, TasinmazBilgi tasinmazBilgi)
        {
            try
            {
                if (id != tasinmazBilgi.Id)
                {
                    return BadRequest();
                }

                var updatedTasinmaz = await _tasinmazBilgiService.UpdateTasinmaz(tasinmazBilgi);

                var log = new Log
                {
                    KullaniciId = tasinmazBilgi.UserId,
                    Durum = "Başarılı",
                    IslemTip = "Güncelleme",
                    Aciklama = $"Taşınmaz ID: {updatedTasinmaz.Id} güncellendi",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                return Ok(updatedTasinmaz);
            }
            catch (Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = tasinmazBilgi.UserId,
                    Durum = "Başarısız",
                    IslemTip = "Taşınmaz Düzenleme",
                    Aciklama = $"Taşınmaz düzenlenirken hata oluştu.",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                Console.WriteLine($"Error updating Tasinmaz: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ActionResult> DeleteTasinmaz(int id)
        {
            try
            {
                var result = await _tasinmazBilgiService.DeleteTasinmaz(id);
                if (!result)
                {
                    return NotFound();
                }

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini alır
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(); // Kullanıcı ID'si alınamaz veya dönüştürülemezse yetkisiz dönüş yapılır
                }

                var log = new Log
                {
                    KullaniciId = userId, // Kullanıcı ID'sini burada kullanıyoruz
                    Durum = "Başarılı",
                    IslemTip = "Sil",
                    Aciklama = $"Taşınmaz ID: {id} silindi",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);
                return NoContent();
            }
            catch (Exception ex)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini alır
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(); // Kullanıcı ID'si alınamaz veya dönüştürülemezse yetkisiz dönüş yapılır
                }
                var log = new Log
                {
                    KullaniciId = userId,
                    Durum = "Başarısız",
                    IslemTip = "Taşınmaz Silme",
                    Aciklama = $"Taşınmaz silinirken hata oluştu: {ex.Message}",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                Console.WriteLine($"Error deleting Tasinmaz: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TasinmazBilgi>>> SearchTasinmaz([FromQuery] string keyword)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var isAdmin = await _authRepository.IsAdmin(userEmail);

                IEnumerable<TasinmazBilgi> tasinmazlar;

                if (isAdmin)
                {
                    tasinmazlar = await _tasinmazBilgiService.SearchAllAsync(keyword);
                }
                else
                {
                    tasinmazlar = await _tasinmazBilgiService.SearchByUserIdAsync(userId, keyword);
                }

                return Ok(tasinmazlar);
            }
            catch (Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    Durum = "Başarısız",
                    IslemTip = "Taşınmaz Arama",
                    Aciklama = $"Taşınmaz aranırken hata oluştu: {ex.Message}",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = await GetUserRoleFromToken()
                };
                await _logService.AddLog(log);

                Console.WriteLine($"Error searching Tasinmaz: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
