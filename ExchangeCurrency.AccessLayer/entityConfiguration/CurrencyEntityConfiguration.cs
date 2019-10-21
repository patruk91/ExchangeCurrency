using ExchangeCurrency.ModelExchangeCurrency.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeCurrency.AccessLayer.entityConfiguration
{
    public class CurrencyEntityConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
        }
    }
}