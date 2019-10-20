﻿using System;
using System.Threading.Tasks;
using ExchangeCurrency.Model;
using ExchangeCurrency.Model.Enums;
using ExchangeCurrency.Model.ExchangeCurrency;
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

            return message;
        }
    }
}