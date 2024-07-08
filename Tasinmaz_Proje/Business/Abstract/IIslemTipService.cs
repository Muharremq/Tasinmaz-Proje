using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface IIslemTipService
    {
        Task<List<IslemTip>> ListIslemTip();
        Task<IslemTip> GetIslemTipById(int id);
        Task AddIslemTip(IslemTip islemTip);
        Task UpdateIslemTip(IslemTip islemTip);
        Task DeleteIslemTip(int id);
    }
}
