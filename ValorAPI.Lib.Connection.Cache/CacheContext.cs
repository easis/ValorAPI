using Microsoft.EntityFrameworkCore;
using System;

namespace ValorAPI.Lib.Connection.Cache
{
    public class CacheContext : DbContext
    {
        public DbSet<CacheItem> CacheItems { get; set; }

        public static string DatabaseName = string.Empty;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={CacheContext.DatabaseName}");
            base.OnConfiguring(optionsBuilder);
        }

        public static void SetDatabaseName(string databaseName)
        {
            CacheContext.DatabaseName = databaseName;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CacheItem>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CacheItem>()
                .HasIndex(i => i.Url)
                .IsUnique();

            modelBuilder.Entity<CacheItem>()
                .Property(i => i.CreatedAt)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
