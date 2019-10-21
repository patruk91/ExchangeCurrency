using System;
using ExchangeCurrency.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeCurrency.AccessLayer.entityConfiguration
{
    public class CurrencyDetailsEntityConfiguration : IEntityTypeConfiguration<CurrencyDetails>
    {
        public void Configure(EntityTypeBuilder<CurrencyDetails> builder)
        {
            builder
                .Property(i => i.CurrencyDetailsId)
                .ValueGeneratedOnAdd();

        }
    }
}