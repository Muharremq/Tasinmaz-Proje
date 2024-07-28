using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using System.Linq;

namespace Tasinmaz_Proje.Services
{
    public class LogService : ILogService
    {
        private readonly TasinmazDbContext _dbContext;

        public LogService (TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Log>> ListLog()
        {
            return await _dbContext.Logs.ToListAsync();
        }

        public async Task<Log> GetLogById(int id)
        {
            return await _dbContext.Logs.FindAsync(id);
        }

        public async Task AddLog (Log log)
        {
            _dbContext.Logs.Add(log);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateLog (Log log)
        {
            _dbContext.Entry(log).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLog (int id)
        {
            var log = await _dbContext.Logs.FindAsync(id);
            if (log != null)
            {
                _dbContext.Logs.Remove(log);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await _dbContext.Logs.ToListAsync();
        }

        public async Task<IEnumerable<Log>> SearchLogsAsync(string term)
        {
            return await _dbContext.Logs
                .Where(l => l.Durum.Contains(term) ||
                            l.IslemTip.Contains(term) ||
                            l.Aciklama.Contains(term) ||
                            l.KullaniciTip.Contains(term))
                .ToListAsync();
        }
    }
}
