using ExchangeCurrency.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchange _exchange;
        private readonly string _codesForExchangeRates;

        public ExchangeController(IExchange exchange, string codesForExchangeRates)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
        }

        [HttpGet]
        public string GetCodesForCurrencies()
        {
            const string message = "Available code currencies for conversions:\n";
            return message + _codesForExchangeRates;
        }

        [HttpGet (template:"{rates}")]
        public string GetRatesForCurrencies()
        {
            const string message = "Current exchange rates (currency to PLN):\n";
            var exchangeRatesData = _exchange.GetExchangeRatesData(ApiBankConfiguration.UriStringToNbpApi,
                ApiBankConfiguration.RequestUriToGetCurrentExchangeRates).Result;
            var exchangeRates = _exchange.GetExchangeRates(exchangeRatesData);
            return message + exchangeRates;
        }
    }
}