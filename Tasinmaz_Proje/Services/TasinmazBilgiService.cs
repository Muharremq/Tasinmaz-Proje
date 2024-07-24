using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Business.Abstract;
using Tasinmaz_Proje.DataAccess;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public class TasinmazBilgiService : ITasinmazBilgiService
    {
        private readonly TasinmazDbContext _dbContext;

        public TasinmazBilgiService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TasinmazBilgi>> GetAllTasinmazBilgi()
        {
            return await _dbContext.Tasinmazlar
                .Include(t => t.Mahalle)
                .ThenInclude(t => t.Ilce)
                .ThenInclude(t => t.Il).Include(t => t.User)
                .ToListAsync();
        }

        public async Task<TasinmazBilgi> GetTasinmazBilgiById(int id)
        {
            return await _dbContext.Tasinmazlar
                .Include(t => t.Mahalle)
                .ThenInclude(t => t.Ilce)
                .ThenInclude(t => t.Il)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TasinmazBilgi> AddTasinmaz(TasinmazBilgi tasinmazBilgi)
        {
            _dbContext.Tasinmazlar.Add(tasinmazBilgi);
            await _dbContext.SaveChangesAsync();
            return tasinmazBilgi;
        }

        public async Task<TasinmazBilgi> UpdateTasinmaz(TasinmazBilgi tasinmazBilgi)
        {
            _dbContext.Tasinmazlar.Update(tasinmazBilgi);
            await _dbContext.SaveChangesAsync();
            return tasinmazBilgi;
        }

        public async Task<bool> DeleteTasinmaz(int id)
        {
            var tasinmazBilgi = await _dbContext.Tasinmazlar.FindAsync(id);
            if(tasinmazBilgi == null)
                return false;

            _dbContext.Tasinmazlar.Remove(tasinmazBilgi);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
