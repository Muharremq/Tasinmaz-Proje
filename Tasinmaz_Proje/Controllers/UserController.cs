using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Services;

namespace Tasinmaz_Proje.Controllers
{
    [Authorize(Roles = "admin")]
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
            try
            {
                var createdUser = await _userService.AddUser(user);

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
            }catch (Exception ex) 
            {
                var log = new Log
                {
                    KullaniciId = user.Id,
                    Durum = "Başarısız",
                    IslemTip = "Kullanıcı Ekleme",
                    Aciklama = $"Kullanıcı eklenirkern hata oluştur.",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = "Admin"
                };
                await _logService.AddLog(log);
            }
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            try
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
                    Aciklama = $"Kullanıcı ID: {user.Id} başarılı bir şekilde güncellendi",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = "Admin"
                };
                await _logService.AddLog(log);
            }catch(Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = user.Id,
                    Durum = "Başarısız",
                    IslemTip = "Güncelleme",
                    Aciklama = $"Kullanıcı ID: {user.Id} silinirken bir sorun oluştu",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = "Admin"
                };
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser (int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                var log = new Log
                {
                    KullaniciId = id,
                    Durum = "Başarılı",
                    IslemTip = "Silme",
                    Aciklama = $"Kullanıcı ID: {id} başarılı bir şekilde silindi",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = "Admin"
                };
                await _logService.AddLog(log);
            }catch (Exception ex)
            {
                var log = new Log
                {
                    KullaniciId = id,
                    Durum = "Başarısız",
                    IslemTip = "Silme",
                    Aciklama = $"Kullanıcı ID: {id} silinirken bir sorun oluştu",
                    TarihveSaat = DateTime.Now,
                    KullaniciTip = "Admin"
                };
                await _logService.AddLog(log);
            }
            return NoContent();
        }
    }
}
