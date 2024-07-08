using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;

namespace Tasinmaz_Proje.Services
{
    public class UserService : IUserService
    {
        private readonly TasinmazDbContext _dbContext;
        
        public UserService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<User>> ListUser()
        {
            return await _dbContext.Users.ToListAsync();
        }
        public async Task<User> GetUserById (int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
        public async Task AddUser (User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateUser (User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteUser (int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
