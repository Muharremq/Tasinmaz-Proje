using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IUserService
    {
        Task<List<User>> ListUser();
        Task<User> GetUserById(int id);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
