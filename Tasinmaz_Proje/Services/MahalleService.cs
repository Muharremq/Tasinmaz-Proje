using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

namespace Tasinmaz_Proje.Services
{
    public class MahalleService : IMahalleService
    {
        private readonly TasinmazDbContext _dbContext;

        public MahalleService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Mahalle>> ListMahalle()
        {
            return await _dbContext.Mahalleler.ToListAsync();
        }

        public async Task<Mahalle> GetMahalleById( int id)
        {
            return await _dbContext.Mahalleler.FindAsync(id);
        }

        public async Task AddMahalle ( Mahalle mahalle)
        {
            _dbContext.Mahalleler.Add(mahalle);
            await _dbContext.SaveChangesAsync();
        }
    }
}
