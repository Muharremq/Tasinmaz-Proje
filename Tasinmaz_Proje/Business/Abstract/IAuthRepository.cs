using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Business.Abstract
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<User> GetUserById(int id);
        Task<bool> IsAdmin(string email);
    }
}
