using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IDurumService
    {
        Task<List<Durum>> ListDurum();
        Task<Durum> GetDurumById(int id);
        Task AddDurum(Durum durum);
        Task UpdateDurum(Durum durum);
        Task DeleteDurum(int id);
    }
}
