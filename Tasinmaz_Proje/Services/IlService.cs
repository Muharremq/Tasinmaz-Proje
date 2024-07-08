using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

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

    }
}
