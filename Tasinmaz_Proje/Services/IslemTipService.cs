using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

namespace Tasinmaz_Proje.Services
{
    public class IslemTipService : IIslemTipService
    {
        private readonly TasinmazDbContext _dbContext;
        public IslemTipService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<IslemTip>> ListIslemTip()
        {
            return await _dbContext.IslemTipleri.ToListAsync();
        }
        public async Task<IslemTip> GetIslemTipById(int id)
        {
            return await _dbContext.IslemTipleri.FindAsync(id);
        }

        public async Task AddIslemTip (IslemTip islemTip)
        {
            _dbContext.IslemTipleri.Add(islemTip);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateIslemTip (IslemTip islemTip)
        {
            _dbContext.Entry(islemTip).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteIslemTip (int id)
        {
            var islemTip = await _dbContext.IslemTipleri.FindAsync (id);
            if(islemTip != null)
            {
                _dbContext.IslemTipleri.Remove(islemTip);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
