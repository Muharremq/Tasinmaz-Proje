using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

namespace Tasinmaz_Proje.Services
{
    public class DurumService : IDurumService
    {
        private readonly TasinmazDbContext _dbContext;

        public DurumService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Durum>> ListDurum()
        {
            return await _dbContext.Durumlar.ToListAsync();
        }

        public async Task<Durum> GetDurumById(int id)
        {
            return await _dbContext.Durumlar.FindAsync(id);
        }

        public async Task AddDurum (Durum durum)
        {
            _dbContext.Durumlar.Add(durum);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDurum (Durum durum)
        {
            _dbContext.Entry(durum).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDurum (int id)
        {
            var durum = await _dbContext.Durumlar.FindAsync (id);
            if (durum != null)
            {
                _dbContext.Durumlar.Remove(durum);
                await _dbContext.SaveChangesAsync ();
            }
        }
    }
}
