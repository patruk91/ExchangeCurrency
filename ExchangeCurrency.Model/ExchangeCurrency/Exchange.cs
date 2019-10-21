using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ExchangeCurrency.Model.Enums;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public class Exchange : IExchange
    {
        private readonly ExchangeHelper _exchangeHelper;
        private readonly StringBuilder _stringBuilder;

        public Exchange(ExchangeHelper exchangeHelper, StringBuilder stringBuilder)
        {
            this._exchangeHelper = exchangeHelper;
            this._stringBuilder = stringBuilder;
        }

        public async Task<HttpResponseMessage> GetStatusCode(string uriString, string requestUri)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync(requestUri);
            }
            return response;
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
                if (response.IsSuccessStatusCode)
                {
                    exchangeRates = response.Content.ReadAsStringAsync();
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            return exchangeRates.Result;
        }

        public Dictionary<string, int> GetCodesForExchangeRates(string exchangeData)
        {
            _stringBuilder.Clear();
            var currencies = _exchangeHelper.GetDataForCurrencies(exchangeData);
            _exchangeHelper.AddCodes(currencies, _stringBuilder);
            var strCodes = _stringBuilder.ToString().Split(",");
            var codes = new Dictionary<string, int>();
            for (var i = 0; i < strCodes.Length; i++)
            {
                codes.Add(strCodes[i], i + 1);
            }
            return codes;
        }

        public string GetExchangeRates(string exchangeData)
        {
            _stringBuilder.Clear();
            var currencies = _exchangeHelper.GetDataForCurrencies(exchangeData);
            _exchangeHelper.AddExchangeRates(currencies, _stringBuilder);

            return _stringBuilder.ToString();
        }

        public decimal CalculateExchange(int amount, string dataFromCurrency, string dataToCurrency, string currency)
        {
            var toCurrency = _exchangeHelper.GetAverageExchangeRate(dataToCurrency);
            var fromCurrency = _exchangeHelper.GetAverageExchangeRate(dataFromCurrency);
            return fromCurrency / toCurrency;
        }

        public Conversions GetConversions(string exchangeRateDataFrom, string exchangeRateDataTo, int amount, Currency currencyFrom, Currency currencyTo)
        {
            var currencyDataFrom = JObject.Parse(exchangeRateDataFrom);
            var currencyDataTo = JObject.Parse(exchangeRateDataTo);
            var effectiveDate = currencyDataFrom["rates"].First["effectiveDate"] + " " + DateTime.Now.ToString("h:mm:ss");
            var dataTransaction = DateTime.Parse(effectiveDate);
            
            var exchangeDataFrom = currencyDataFrom["rates"].First["mid"].ToString();
            decimal.TryParse(exchangeDataFrom, out var exchangeRateFrom);
            var exchangeDataTo = currencyDataTo["rates"].First["mid"].ToString();
            decimal.TryParse(exchangeDataTo, out var exchangeRateTo);

            var ratio = CalculateRatio(exchangeRateFrom, exchangeRateTo);
            var result = CalculateResult(amount, ratio);

            return new Conversions(dataTransaction, currencyFrom, amount, currencyTo, result, ratio);
        }

        private decimal CalculateRatio(decimal exchangeRateFrom, decimal exchangeRateTo)
        {
            return exchangeRateFrom / exchangeRateTo;
        }

        private decimal CalculateResult(int amount, decimal ratio)
        {
            return Math.Round(ratio * amount, decimals: 2, MidpointRounding.AwayFromZero);
        }
    }


}
