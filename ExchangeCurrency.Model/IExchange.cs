using System.Threading.Tasks;

namespace ExchangeCurrency.Model
{
    public interface IExchange
    {
        Task<string> GetExchangeRatesData(string uriString, string requestUri);
        string GetCodesForExchangeRates(string currentExchangeRates);
        string GetExchangeRates(string currentExchangeRates);
        decimal CalculateExchange(int amount, string exchangeRateDataFromCurrency, string exchangeRateDataToCurrency, string fromCurrency);
    }
}