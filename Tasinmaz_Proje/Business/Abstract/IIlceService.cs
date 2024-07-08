using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IIlceService
    {
        Task<List<Ilce>> ListIlce();
        Task<Ilce> GetIlceById(int id);
        Task AddIlce(Ilce ilce);

    }
}
