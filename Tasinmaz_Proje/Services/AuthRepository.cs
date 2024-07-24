﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tasinmaz_Proje.Business.Abstract;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using System;

namespace Tasinmaz_Proje.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TasinmazDbContext _dbContext;

        public AuthRepository(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void CreatePasswordHash ( string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Register (User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] UserPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for( int i = 0; i < computeHash.Length; i++ )
                {
                    if (computeHash[i] != UserPasswordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<User> Login ( string Email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == Email);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;    
            }
            return user;
        }

        public async Task<bool> UserExists (string Email)
        {
            if(await _dbContext.Users.AnyAsync(x => x.Email == Email))
            {
                return true;
            }
            return false;
        }
    }
}
