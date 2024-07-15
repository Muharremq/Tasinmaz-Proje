using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using System.IO;
using System.Text.Json;
using System;
using Newtonsoft.Json;

namespace Tasinmaz_Proje.Services
{
    public class IlService : IIlService
    {
        private readonly TasinmazDbContext _context;

        public IlService(TasinmazDbContext context)
        {
            _context = context;
        }
        public async Task<List<Il>> ListIl()
        {
            return await _context.Iller.ToListAsync();
        }
        public async Task<Il> GetIlById(int id)
        {
            return await _context.Iller.FindAsync(id);
        }
        public async Task AddIl (Il il)
        {
            _context.Iller.Add(il);
            await _context.SaveChangesAsync();
        }

        public async Task AddIllerFromJsonFileAsync(string filePath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var iller = JsonConvert.DeserializeObject<List<Il>>(json);

                foreach (var il in iller)
                {
                    if (string.IsNullOrWhiteSpace(il.Name))
                    {
                        // `Name` alanı boş veya null ise, işlemi atla veya hata fırlat.
                        throw new Exception($"Il entity's Name cannot be null or empty. Il ID: {il.Id}");
                    }

                    // Veritabanında mevcut ID'yi kontrol et.
                    var existingIl = await _context.Iller.FindAsync(il.Id);

                    if (existingIl != null)
                    {
                        // Mevcut ID bulundu, güncelle.
                        existingIl.Name = il.Name;
                        _context.Iller.Update(existingIl);
                    }
                    else
                    {
                        // Mevcut ID bulunamadı, yeni kayıt ekle.
                        var newIl = new Il
                        {
                            Id = il.Id,
                            Name = il.Name
                        };

                        await _context.Iller.AddAsync(newIl);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata mesajını loglayın veya uygun bir şekilde ele alın.
                throw new Exception("Error adding or updating iller from JSON file", ex);
            }
        }




        public async Task DeleteIl(int id)
        {
            var il = await _context.Iller.FindAsync(id);
            if (il != null)
            {
                _context.Iller.Remove(il);
                await _context.SaveChangesAsync();
            }
        }

    }
    }
