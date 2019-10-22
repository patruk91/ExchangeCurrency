using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer;
using ExchangeCurrency.AccessLayer.dao;
using ExchangeCurrency.Model;
using ExchangeCurrency.Model.ExchangeCurrency;
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
        private Dictionary<string, int> _codesForExchangeRates;
        private readonly ExchangeDbEntities _context;
        private readonly IConversionDao _conversionDao;
        private readonly ICurrencyDao _currencyDao;
        private HttpStatusCode _statusCode;
        private readonly ApiConnections _apiConnections;
        private readonly ExchangeHelper _exchangeHelper;

        public ExchangeController(IExchange exchange,
                                Dictionary<string, int> codesForExchangeRates,
                                ExchangeDbEntities context,
                                IConversionDao conversionDao,
                                ICurrencyDao currencyDao,
                                HttpStatusCode statusCode,
                                ApiConnections apiConnections,
                                ExchangeHelper exchangeHelper)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
            _context = context;
            _conversionDao = conversionDao;
            _currencyDao = currencyDao;
            _statusCode = statusCode;
            _apiConnections = apiConnections;
            _exchangeHelper = exchangeHelper;
        }

        [HttpGet]
        public IActionResult GetCodesForCurrencies()
        {
            if (!_codesForExchangeRates.Any())
            {
                try
                {
                    _codesForExchangeRates = _exchangeHelper.LoadCodeCurrencies(_exchange, _apiConnections.UriString,
                        _apiConnections.RequestUriAllRates);
                }
                catch (StatusCodeException e)
                {
                    var codeNumber = e.CodeNumber;
                    StatusCodeResponses.GetResponseMessage(codeNumber);
                }
            }
            const string message = "Available code currencies for conversions:\n";
            return Ok(message + string.Join(",", _codesForExchangeRates.Keys));

        }

        [HttpGet (template:"{rates}")]
        public async Task<IActionResult> GetRatesForCurrencies()
        {
            var exchangeRatesData = await _exchange.GetExchangeRatesData(_apiConnections.UriString,
                                                                        _apiConnections.RequestUriAllRates);
            var exchangeRates = _exchange.GetExchangeRates(exchangeRatesData);

            const string message = "Current exchange rates (currency to PLN):\n";
            return Ok(message + exchangeRates);
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