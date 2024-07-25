using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Business.Abstract
{
    public interface ITasinmazBilgiService
    {
        Task<IEnumerable<TasinmazBilgi>> GetAllTasinmazBilgi();

        Task<TasinmazBilgi> GetTasinmazBilgiById(int id);
        Task<TasinmazBilgi> AddTasinmaz(TasinmazBilgi tasinmazBilgi);
        Task<TasinmazBilgi> UpdateTasinmaz(TasinmazBilgi tasinmazBilgi);
        Task<bool> DeleteTasinmaz(int id);
        Task<List<TasinmazBilgi>> GetTasinmazlarByUserId(int userId);


    }
}
