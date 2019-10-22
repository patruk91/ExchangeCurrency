using System;
using ExchangeCurrency.ModelExchangeCurrency.Enums;

namespace ExchangeCurrency.ModelExchangeCurrency.Models
{
    public class Conversions
    {
        public int Id { get; set; }
        public DateTime DataTransaction { get; set; }
        public int AmountFrom { get; set; }
        public decimal Result { get; set; }
        public decimal Rate { get; set; }

        public Currency CurrencyFrom { get; set; }
        public Currency CurrencyTo { get; set; }

        public Conversions() { }

        public Conversions(DateTime dataTransaction, Currency currencyFrom, int amountFrom, Currency currencyTo, decimal result, decimal rate)
        {
            DataTransaction = dataTransaction;
            CurrencyFrom = currencyFrom;
            AmountFrom = amountFrom;
            CurrencyTo = currencyTo;
            Result = result;
            Rate = rate;
        }
    }
}