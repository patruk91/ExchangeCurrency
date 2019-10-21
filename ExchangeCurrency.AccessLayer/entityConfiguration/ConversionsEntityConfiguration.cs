using System;
using ExchangeCurrency.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeCurrency.AccessLayer.entityConfiguration
{
    public class ConversionsEntityConfiguration : IEntityTypeConfiguration<Conversions>
    {
        public void Configure(EntityTypeBuilder<Conversions> builder)
        {
            builder
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne<Currency>(c => c.CurrencyTo)
                .WithMany(v => v.Conversions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Currency>(c => c.CurrencyTo)
                .WithMany(v => v.Conversions)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}