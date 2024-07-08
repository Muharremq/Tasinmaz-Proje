using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

namespace Tasinmaz_Proje.Services
{
    public class IlceService : IIlceService
    {
        private readonly TasinmazDbContext _dbContext;
        public IlceService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Ilce>> ListIlce()
        {
            return await _dbContext.Ilceler.ToListAsync();
        }
        public async Task<Ilce> GetIlceById(int id)
        {
            return await _dbContext.Ilceler.FindAsync(id);
        }
        public async Task AddIlce (Ilce ilce)
        {
            _dbContext.Ilceler.Add(ilce);
            await _dbContext.SaveChangesAsync();
        }
    }
}
