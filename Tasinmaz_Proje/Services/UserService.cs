﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using System.Linq;

namespace Tasinmaz_Proje.Services
{
    public class UserService : IUserService
    {
        private readonly TasinmazDbContext _context;
        
        public UserService(TasinmazDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<User>> ListUser()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserById (int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUser (User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser (int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync (string query)
        {
            return await _context.Users
                .Where(l => l.Name.Contains(query) ||
                            l.Surname.Contains(query) ||
                            l.Email.Contains(query) ||
                            l.Phone.Contains(query) ||
                            l.Role.Contains(query))
                .ToListAsync();

        }
    }
}
