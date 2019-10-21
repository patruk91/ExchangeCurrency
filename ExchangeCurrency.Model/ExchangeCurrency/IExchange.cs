using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public interface IExchange
    {
        Task<string> GetExchangeRatesData(string uriString, string requestUri);
        Dictionary<string, int> GetCodesForExchangeRates(string currentExchangeRates);
        string GetExchangeRates(string currentExchangeRates);
        decimal CalculateExchange(int amount, string dataFromCurrency, string dataToCurrency, string fromCurrency);
        Conversions GetConversions(string exchangeRateDataFrom, string exchangeRateDataTo, int amount,
            Currency currencyFrom, Currency currencyTo);

        Task<HttpResponseMessage> GetStatusCode(string uriString, string requestUri);
    }
}