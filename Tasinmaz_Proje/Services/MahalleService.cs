using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.DataAccess;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

namespace Tasinmaz_Proje.Services
{
    public class MahalleService : IMahalleService
    {
        private readonly TasinmazDbContext _dbContext;

        public MahalleService(TasinmazDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Mahalle>> ListMahalle()
        {
            return await _dbContext.Mahalleler.ToListAsync();
        }

        public async Task<Mahalle> GetMahalleById( int id)
        {
            return await _dbContext.Mahalleler.FindAsync(id);
        }

        public async Task AddMahalle ( Mahalle mahalle)
        {
            _dbContext.Mahalleler.Add(mahalle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddNeighborhoodsFromJsonFileAsync(string filePath)
        {
            try
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = await r.ReadToEndAsync();
                    var neighborhoodsData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(json);

                    foreach (var ilKey in neighborhoodsData.Keys)
                    {
                        // İl Id'sini int olarak alalım
                        if (!int.TryParse(ilKey, out int ilId))
                        {
                            // İl Id'si geçersiz, log veya hata işlemleri yapılabilir
                            continue;
                        }

                        // İl Id'sine göre veritabanında ilin varlığını kontrol et
                        var il = await _dbContext.Iller.FindAsync(ilId);
                        if (il == null)
                        {
                            // İl bulunamadı, log veya hata işlemleri yapılabilir
                            continue;
                        }

                        // İlçe ve mahalleleri ekleyelim
                        var ilceListesi = neighborhoodsData[ilKey];
                        foreach (var ilceKey in ilceListesi.Keys)
                        {
                            var ilce = await _dbContext.Ilceler
                                .FirstOrDefaultAsync(i => i.Name == ilceKey && i.IlId == ilId);

                            if (ilce == null)
                            {
                                ilce = new Ilce { Name = ilceKey, IlId = ilId };
                                _dbContext.Ilceler.Add(ilce);
                                await _dbContext.SaveChangesAsync(); // Yeni ilçe kaydedildi
                            }

                            var mahalleListesi = ilceListesi[ilceKey];
                            foreach (var mahalleName in mahalleListesi)
                            {
                                var mahalle = await _dbContext.Mahalleler
                                    .FirstOrDefaultAsync(m => m.Name == mahalleName && m.IlceId == ilce.Id);

                                if (mahalle == null)
                                {
                                    mahalle = new Mahalle { Name = mahalleName, IlceId = ilce.Id };
                                    _dbContext.Mahalleler.Add(mahalle);
                                }
                            }
                        }

                        await _dbContext.SaveChangesAsync(); // Değişiklikleri kaydet
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya hata işlemleri yapılabilir
                throw new Exception($"Error importing neighborhoods: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Mahalle>> GetAllMahallelerAsync()
        {
            return await _dbContext.Mahalleler.ToListAsync();
        }

        public async Task<List<Mahalle>> GetMahallelerByIlceId(int ilceId)
        {
            return await _dbContext.Mahalleler.Where(mahalle => mahalle.IlceId == ilceId).ToListAsync();
        }
    }
}
