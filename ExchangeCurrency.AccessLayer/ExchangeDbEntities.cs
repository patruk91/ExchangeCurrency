using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer.entityConfiguration;
using ExchangeCurrency.Model.Models;
using Microsoft.Extensions.Configuration;

namespace ExchangeCurrency.AccessLayer
{
    public sealed class ExchangeDbEntities : DbContext
    {
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Conversions> Conversions { get; set; }
        private readonly Dictionary<string, int> _codesForExchangeRates;

        public ExchangeDbEntities(DbContextOptions<ExchangeDbEntities> options, Dictionary<string, int> codesForExchangeRates) : base(options)
        {
            if (Database.EnsureCreated())
            {
                _codesForExchangeRates = codesForExchangeRates;
                PopulateDb();
            }
        }

        public void PopulateDb()
        {
            foreach (var code in _codesForExchangeRates.Keys)
            {
                this.Currency.Add(new Currency(code));
                this.SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ConversionsEntityConfiguration());
        }
    }

}
