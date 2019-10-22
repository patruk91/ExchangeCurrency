using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer;
using ExchangeCurrency.AccessLayer.dao;
using ExchangeCurrency.ModelExchangeCurrency;
using ExchangeCurrency.ModelExchangeCurrency.Enums;
using ExchangeCurrency.ModelExchangeCurrency.ExchangeCurrency;
using ExchangeCurrency.ModelExchangeCurrency.Models;
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
        private readonly ICurrencyDao _currencyDao;
        private readonly HttpStatusCode _statusCode;
        private readonly ApiConnections _apiConnections;

        public ExchangeController(IExchange exchange,
                                Dictionary<string, int> codesForExchangeRates,
                                ExchangeDbEntities context,
                                IConversionDao conversionDao,
                                ICurrencyDao currencyDao,
                                HttpStatusCode statusCode,
                                ApiConnections apiConnections)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
            _context = context;
            _conversionDao = conversionDao;
            _currencyDao = currencyDao;
            _statusCode = statusCode;
            _apiConnections = apiConnections;
        }

        [HttpGet]
        public IActionResult GetCodesForCurrencies()
        {
            if (_statusCode == HttpStatusCode.OK && _codesForExchangeRates.Any())
            {
                const string message = "Available code currencies for conversions:\n";
                return Ok(message + string.Join(",", _codesForExchangeRates.Keys));
            }
            return StatusCode(500, "Service temporary unavailable. Please try again later.\n");
        }

        [HttpGet (template:"{rates}")]
        public async Task<string> GetRatesForCurrencies()
        {
            var exchangeRatesData = await _exchange.GetExchangeRatesData(_apiConnections.UriString,
                                                                        _apiConnections.RequestUriAllRates);
            var exchangeRates = _exchange.GetExchangeRates(exchangeRatesData);

            const string message = "Current exchange rates (currency to PLN):\n";
            return message + exchangeRates;
        }

        [HttpGet(template: "{amount}/{fromCurrency}/{toCurrency}")]
        public async Task<string> GetCalculatedExchangeForCurrencies(int amount, string fromCurrency, string toCurrency)
        {
            var uriString = _apiConnections.UriString;
            var dataFromCurrency = await _exchange.GetExchangeRatesData(uriString, _apiConnections.RequestUriToSingleRate(fromCurrency));
            var currencyFrom = _currencyDao.GetCurrency(fromCurrency, _context, _codesForExchangeRates);

            var dataToCurrency = await _exchange.GetExchangeRatesData(uriString, _apiConnections.RequestUriToSingleRate(toCurrency));
            var currencyTo = _currencyDao.GetCurrency(toCurrency, _context, _codesForExchangeRates);

            var conversions = _exchange.GetConversionsDetails(dataFromCurrency, dataToCurrency, amount, currencyFrom, currencyTo);
            await _conversionDao.AddConversions(conversions, _context);

            return $"{amount}{fromCurrency} = {conversions.Result}{toCurrency}:\n";
        }
    }
}