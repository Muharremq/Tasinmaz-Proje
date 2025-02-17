﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Il>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<TasinmazBilgi>()
                    .HasOne(t => t.User)
                    .WithMany(u => u.Tasinmazlar)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
