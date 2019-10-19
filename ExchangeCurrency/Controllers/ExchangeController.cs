using ExchangeCurrency.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchange _nbpHandler;
        private readonly string _codeCurrencies;

        public ExchangeController(IExchange nbpHandler, string codeCurrencies)
        {
            _nbpHandler = nbpHandler;
            _codeCurrencies = codeCurrencies;
        }


        [HttpGet]
        public string Get()
        {
            const string message = "Available currencies for conversions:\n";
            return message + _codeCurrencies;
        }

        [HttpGet (template:"{rates}")]
        public string GetCurrencyRates()
        {
            const string message = "Current exchange rates (currency to PLN):\n";
            var currentExchangeRates = _nbpHandler.GetCurrentExchangeRates(ApiBankConfiguration.UriStringToNbpApi,
                ApiBankConfiguration.RequestUriToGetCurrentExchangeRates).Result;
            var exchangeRates = _nbpHandler.GetExchangeRates(currentExchangeRates);
            return message + exchangeRates;
        }


    }
}