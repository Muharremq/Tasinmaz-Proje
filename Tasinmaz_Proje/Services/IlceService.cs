using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Tasinmaz_Proje.Services
{
    public class IlceService : IIlceService
    {
        private readonly TasinmazDbContext _dbContext;
        public IlceService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Ilce>> ListIlce()
        {
            return await _dbContext.Ilceler.ToListAsync();
        }
        public async Task<Ilce> GetIlceById(int id)
        {
            return await _dbContext.Ilceler.FindAsync(id);
        }
        public async Task AddIlce (Ilce ilce)
        {
            _dbContext.Ilceler.Add(ilce);
            await _dbContext.SaveChangesAsync();
        }
        // IlService.cs

        // Add this method to your IlService class
        public async Task AddDistrictsFromJsonFileAsync(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = await r.ReadToEndAsync();
                var districts = JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(json);

                foreach (var (ilId, districtNames) in districts)
                {
                    var city = await _dbContext.Iller.FindAsync(ilId);

                    if (city != null)
                    {
                        foreach (var districtName in districtNames)
                        {
                            var newDistrict = new Ilce
                            {
                                Name = districtName,
                                IlId = city.Id
                            };

                            _dbContext.Ilceler.Add(newDistrict);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Ilce>> GetIlcelerByIlId(int ilId)
        {
            return await _dbContext.Ilceler.Where(ilce => ilce.IlId == ilId).ToListAsync();
        }

    }
}
