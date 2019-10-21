using Microsoft.EntityFrameworkCore;
using System;
using ExchangeCurrency.AccessLayer.entityConfiguration;
using ExchangeCurrency.Model.Models;
using Microsoft.Extensions.Configuration;

namespace ExchangeCurrency.AccessLayer
{
    public class ExchangeDbEntities : DbContext
    {
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyDetails> CurrencyDetails { get; set; }

        public ExchangeDbEntities(DbContextOptions<ExchangeDbEntities> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyDetailsEntityConfiguration());
        }
    }

}
