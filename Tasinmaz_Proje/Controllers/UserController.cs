using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UserController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _userService.ListUser();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById (int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser (User user)
        {
            await _userService.AddUser(user);

            var log = new Log
            {
                KullaniciId = user.Id,
                Durum = "Başarılı",
                IslemTip = "Ekleme",
                Aciklama = $"Kullanıcı: {user.Id} eklendi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };

            await _logService.AddLog(log);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userService.UpdateUser(user);

            var log = new Log
            {
                KullaniciId = user.Id,
                Durum = "Başarılı",
                IslemTip = "Güncelleme",
                Aciklama = $"Kullanıcı ID: {user.Id} güncellendi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };
            await _logService.AddLog(log);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser (int id)
        {
            await _userService.DeleteUser(id);

            var log = new Log
            {
                KullaniciId = id,
                Durum = "Başarılı",
                IslemTip = "Silme",
                Aciklama = $"Taşınmaz ID: {id} silindi",
                TarihveSaat = DateTime.Now,
                KullaniciTip = "Admin"
            };
            await _logService.AddLog(log);

            return NoContent();
        }
    }
}
