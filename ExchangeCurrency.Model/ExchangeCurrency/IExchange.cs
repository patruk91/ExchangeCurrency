using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeCurrency.ModelExchangeCurrency.Models;

namespace ExchangeCurrency.ModelExchangeCurrency.ExchangeCurrency
{
    public interface IExchange
    {
        Task<string> GetExchangeRatesData(string uriString, string requestUri);
        Dictionary<string, int> GetCodesForExchangeRates(string currentExchangeRates);
        string GetExchangeRates(string currentExchangeRates);
        Conversions GetConversionsDetails(string exchangeRateDataFrom, string exchangeRateDataTo, int amount,
            Currency currencyFrom, Currency currencyTo);
        Task<HttpResponseMessage> GetStatusCode(string uriString, string requestUri);
    }
}