using System;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer;
using ExchangeCurrency.AccessLayer.dao;
using ExchangeCurrency.Model;
using ExchangeCurrency.Model.Enums;
using ExchangeCurrency.Model.ExchangeCurrency;
using ExchangeCurrency.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCurrency.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchange _exchange;
        private readonly string _codesForExchangeRates;
        private readonly ExchangeDbEntities _context;
        private ICurrencyDao _currencyDao;
        private ICurrencyDetailsDao _currencyDetailsDao;

        public ExchangeController(IExchange exchange, string codesForExchangeRates, ExchangeDbEntities context, ICurrencyDao currencyDao, ICurrencyDetailsDao currencyDetailsDao)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
            _context = context;
            _currencyDao = currencyDao;
            _currencyDetailsDao = currencyDetailsDao;
        }

        [HttpGet]
        public string GetCodesForCurrencies()
        {
            const string message = "Available code currencies for conversions:\n";
            return message + _codesForExchangeRates;
        }

        [HttpGet (template:"{rates}")]
        public async Task<string> GetRatesForCurrencies()
        {
            var uriString = ApiBankConfiguration.GetUriLink(ApiBankConfiguration.UriToNbpApi);
            var requestUri = ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRates,
                TableNames.A.ToString());

            var exchangeRatesData = await _exchange.GetExchangeRatesData(uriString, requestUri);
            var exchangeRates = _exchange.GetExchangeRates(exchangeRatesData);

            const string message = "Current exchange rates (currency to PLN):\n";
            return message + exchangeRates;
        }

        [HttpGet(template: "{amount}/{fromCurrency}/{toCurrency}")]
        public async Task<string> GetCalculatedExchangeForCurrencies(int amount, string fromCurrency, string toCurrency)
        {
            var uriString = ApiBankConfiguration.GetUriLink(ApiBankConfiguration.UriToNbpApi);
            var requestUriFromCurrency = ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRate,
                TableNames.A.ToString(), fromCurrency);
            var requestUriToCurrency = ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRate,
                TableNames.A.ToString(), toCurrency);

            var dataFromCurrency = await _exchange.GetExchangeRatesData(uriString, requestUriFromCurrency);
            var dataToCurrency = await _exchange.GetExchangeRatesData(uriString, requestUriToCurrency);

            var calculatedAmount = _exchange.CalculateExchange(amount, dataFromCurrency, dataToCurrency, fromCurrency);
            var message = $"{amount}{fromCurrency} = {Math.Round(calculatedAmount * amount, decimals:2, MidpointRounding.AwayFromZero)}{toCurrency}:\n";

            CurrencyDetails currencyDetails = _exchange.GetCurrencyDetails(dataFromCurrency);
            Currency currency = _exchange.GetCurrency(dataFromCurrency, currencyDetails);
            await _currencyDao.AddCurrency(currency, _context);
            //await _currencyDetailsDao.AddCurrencyDetails(currencyDetails, _context);

            return message;
        }
    }
}