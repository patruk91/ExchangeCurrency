using System.Threading.Tasks;

namespace ExchangeCurrency.Model
{
    public interface IExchange
    {
        Task<string> GetCurrentExchangeRates(string uriString, string requestUri);
    }
}