using System.Collections.Generic;
using ExchangeCurrency.ModelExchangeCurrency.Models;

namespace ExchangeCurrency.AccessLayer.dao.sql
{
    public class CurrencySql : ICurrencyDao
    {
        public Currency GetCurrency(string currencyData,
                                    ExchangeDbEntities context,
                                    Dictionary<string, int> codesForExchangeRates)
        {
            return context.Currency.Find(codesForExchangeRates[currencyData]);
        }
    }
}