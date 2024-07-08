using Microsoft.EntityFrameworkCore;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.DataAccess
{
    public class TasinmazDbContext : DbContext
    {
        public TasinmazDbContext(DbContextOptions<TasinmazDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TasinmazBilgi> Tasinmazlar { get; set; }
        public DbSet<Il> Iller { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Mahalle> Mahalleler { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Durum> Durumlar { get; set; }
        public DbSet<IslemTip> IslemTipleri { get; set; }
    }
}
