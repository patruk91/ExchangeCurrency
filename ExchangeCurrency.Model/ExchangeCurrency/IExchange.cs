using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public interface IExchange
    {
        Task<string> GetExchangeRatesData(string uriString, string requestUri);
        string GetCodesForExchangeRates(string currentExchangeRates);
        string GetExchangeRates(string currentExchangeRates);
        decimal CalculateExchange(int amount, string dataFromCurrency, string dataToCurrency, string fromCurrency);
        Currency GetCurrency(string exchangeRateData);
        CurrencyDetails GetCurrencyDetails(string exchangeRateData);
    }
}