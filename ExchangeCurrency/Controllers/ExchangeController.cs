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
using Newtonsoft.Json;

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
        private readonly ApiConnections _apiConnections;
        private readonly ExchangeHelper _exchangeHelper;

        public ExchangeController(IExchange exchange,
                                Dictionary<string, int> codesForExchangeRates,
                                ExchangeDbEntities context,
                                IConversionDao conversionDao,
                                ICurrencyDao currencyDao,
                                ApiConnections apiConnections,
                                ExchangeHelper exchangeHelper)
        {
            _exchange = exchange;
            _codesForExchangeRates = codesForExchangeRates;
            _context = context;
            _conversionDao = conversionDao;
            _currencyDao = currencyDao;
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
                    return StatusCode(codeNumber, StatusCodeResponses.GetResponseMessage(codeNumber));
                }
            }

            var codes = JsonConvert.SerializeObject(_codesForExchangeRates.Keys.ToList());
            const string message = "Available code currencies for conversions:\n";
            return Ok(message + string.Join(",", codes));

        }

        [HttpGet (template:"{rates}")]
        public async Task<IActionResult> GetRatesForCurrencies()
        {
            string exchangeRatesData;
            try
            {
                exchangeRatesData = await _exchange.GetExchangeRatesData(_apiConnections.UriString,
                    _apiConnections.RequestUriAllRates);
            }
            catch (StatusCodeException e)
            {
                var codeNumber = e.CodeNumber;
                return StatusCode(codeNumber, StatusCodeResponses.GetResponseMessage(codeNumber));
            }
            var exchangeRates = _exchange.GetExchangeRates(exchangeRatesData);
            const string message = "Current exchange rates (currency to PLN):\n";

            var actualRates = _exchangeHelper.GetActualRates(exchangeRates);
            var codes = JsonConvert.SerializeObject(actualRates);
            return Ok(message + codes);
        }

        [HttpGet(template: "{amount}/{fromCurrency}/{toCurrency}")]
        public async Task<IActionResult> GetCalculatedExchangeForCurrencies(int amount, string fromCurrency, string toCurrency)
        {
            var uriString = _apiConnections.UriString;
            string dataFromCurrency;
            string dataToCurrency;
            try
            {
                dataFromCurrency = await _exchange.GetExchangeRatesData(uriString, _apiConnections.RequestUriToSingleRate(fromCurrency));
                dataToCurrency = await _exchange.GetExchangeRatesData(uriString, _apiConnections.RequestUriToSingleRate(toCurrency));
            }
            catch (StatusCodeException e)
            {
                var codeNumber = e.CodeNumber;
                return StatusCode(codeNumber, StatusCodeResponses.GetResponseMessage(codeNumber));
            }
            var currencyFrom = _currencyDao.GetCurrency(fromCurrency.ToUpper(), _context, _codesForExchangeRates);
            var currencyTo = _currencyDao.GetCurrency(toCurrency.ToUpper(), _context, _codesForExchangeRates);

            var conversions = _exchange.GetConversionsDetails(dataFromCurrency, dataToCurrency, amount, currencyFrom, currencyTo);
            await _conversionDao.AddConversions(conversions, _context);

            return Ok($"{amount}{fromCurrency.ToUpper()} = {conversions.Result}{toCurrency.ToUpper()}:\n");
        }
    }
}