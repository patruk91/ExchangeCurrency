using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ExchangeCurrency.ModelExchangeCurrency.ExchangeCurrency;
using ExchangeCurrency.ModelExchangeCurrency.Models;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public class Exchange : IExchange
    {
        private readonly ExchangeHelper _exchangeHelper;

        public Exchange(ExchangeHelper exchangeHelper)
        {
            this._exchangeHelper = exchangeHelper;
        }

        public async Task<HttpStatusCode> GetStatusCode(string uriString, string requestUri)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(requestUri);
            }
            return response.StatusCode;
        }

        public async Task<string> GetExchangeRatesData(string uriString, string requestUri)
        {
            Task<string> exchangeRates;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(requestUri);
                exchangeRates = response.Content.ReadAsStringAsync();
            }
            return exchangeRates.Result;
        }

        public Dictionary<string, int> GetCodesForExchangeRates(string exchangeData)
        {
            var currencies = _exchangeHelper.GetDataForCurrencies(exchangeData);
            var strCodes = _exchangeHelper.GetCodes(currencies).Split(",");
            var codes = _exchangeHelper.AddCodes(strCodes);

            return codes;
        }

        public string GetExchangeRates(string exchangeData)
        {
            var currencies = _exchangeHelper.GetDataForCurrencies(exchangeData);
            return _exchangeHelper.GetExchangeRates(currencies); ;
        }

        public Conversions GetConversionsDetails(string exchangeRateDataFrom, string exchangeRateDataTo, int amount, Currency currencyFrom, Currency currencyTo)
        {
            var currencyDataFrom = _exchangeHelper.ParseToJObject(exchangeRateDataFrom);
            var currencyDataTo = _exchangeHelper.ParseToJObject(exchangeRateDataTo);
            var dataTransaction = _exchangeHelper.GetDataTransaction(currencyDataFrom);

            var exchangeRateFrom = _exchangeHelper.GetExchangeRate(currencyDataFrom);
            var exchangeRateTo = _exchangeHelper.GetExchangeRate(currencyDataTo); 

            var ratio = _exchangeHelper.CalculateRatio(exchangeRateFrom, exchangeRateTo);
            var result = _exchangeHelper.CalculateResult(amount, ratio);

            return new Conversions(dataTransaction, currencyFrom, amount, currencyTo, result, ratio);
        }


    }
}
