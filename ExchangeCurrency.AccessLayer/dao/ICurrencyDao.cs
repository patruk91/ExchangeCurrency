using System.Collections.Generic;
using ExchangeCurrency.ModelExchangeCurrency.Models;

namespace ExchangeCurrency.AccessLayer.dao
{
    public interface ICurrencyDao
    {
        Currency GetCurrency(string currencyData,
                            ExchangeDbEntities context,
                            Dictionary<string, int> codesForExchangeRates);
    }
}