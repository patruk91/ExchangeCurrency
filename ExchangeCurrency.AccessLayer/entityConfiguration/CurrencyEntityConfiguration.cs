using ExchangeCurrency.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeCurrency.AccessLayer.entityConfiguration
{
    public class CurrencyEntityConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder
                .Property(i => i.CurrencyId)
                .ValueGeneratedOnAdd();

            builder.HasOne<CurrencyDetails>(d => d.Details)
                .WithOne(c => c.Currency)
                .HasForeignKey<CurrencyDetails>(d => d.CurrencyRef);

        }
    }
}