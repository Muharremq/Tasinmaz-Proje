using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IIlService
    {
        Task<List<Il>> ListIl();
        Task<Il> GetIlById(int id);
        Task AddIl(Il il);
        Task AddIllerFromJsonFileAsync(string filePath);
        Task DeleteIl(int id);


    }
}
