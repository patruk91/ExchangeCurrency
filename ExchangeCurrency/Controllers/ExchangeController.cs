using System;
using System.Collections.Generic;
using System.Net;
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
        private readonly Dictionary<string, int> _codesForExchangeRates;
        private readonly ExchangeDbEntities _context;
        private readonly IConversionDao _conversionDao;
        private readonly HttpStatusCode _statusCode;

        public ExchangeController(IExchange exchange,
                                Dictionary<string, int> codesForExchangeRates,
                                ExchangeDbEntities context,
                                IConversionDao conversionDao,
                                HttpStatusCode statusCode)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
            _context = context;
            _conversionDao = conversionDao;
            _statusCode = statusCode;
        }

        [HttpGet]
        public string GetCodesForCurrencies()
        {
            const string message = "Available code currencies for conversions:\n";
            return message + string.Join(",",_codesForExchangeRates.Keys);
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

            var currencyFrom = _context.Currency.Find(_codesForExchangeRates[fromCurrency]);
            var currencyTo = _context.Currency.Find(_codesForExchangeRates[toCurrency]);

            var conversions = _exchange.GetConversions(dataFromCurrency, dataToCurrency, amount, currencyFrom, currencyTo);
            await _conversionDao.AddConversions(conversions, _context);

            return $"{amount}{fromCurrency} = {conversions.Result}{toCurrency}:\n";
        }
    }
}