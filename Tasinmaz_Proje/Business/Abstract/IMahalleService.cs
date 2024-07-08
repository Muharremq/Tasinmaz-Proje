using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IMahalleService
    {
        Task<List<Mahalle>> ListMahalle();
        Task<Mahalle> GetMahalleById(int id);
        Task AddMahalle (Mahalle mahalle);

    }
}
