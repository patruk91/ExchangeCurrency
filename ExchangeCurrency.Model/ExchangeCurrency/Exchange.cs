using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public string GetCodesForExchangeRates(string exchangeData)
        {
            _stringBuilder.Clear();
            var currencies = _exchangeHelper.GetDataForCurrencies(exchangeData);
            _exchangeHelper.AddCodes(currencies, _stringBuilder);

            return _stringBuilder.ToString();
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

    }


}
