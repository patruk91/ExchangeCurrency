using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExchangeCurrency.Model
{
    public class Exchange : IExchange
    {
        private readonly StringBuilder _stringBuilder;

        public Exchange()
        {
            this._stringBuilder = new StringBuilder();
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
            var currencies = GetCurrenciesData(exchangeData);
            AddCodes(currencies);

            return _stringBuilder.ToString();
        }

        private void AddCodes(JToken currencies)
        {
            var prefix = "";
            foreach (var currency in currencies)
            {
                _stringBuilder.Append(prefix);
                prefix = ",";
                _stringBuilder.Append(currency["code"]);
            }
        }

        public string GetExchangeRates(string exchangeData)
        {
            _stringBuilder.Clear();
            var currencies = GetCurrenciesData(exchangeData);
            AddExchangeRates(currencies);

            return _stringBuilder.ToString();
        }

        private void AddExchangeRates(JToken currencies)
        {
            foreach (var currency in currencies)
            {
                _stringBuilder.Append(currency["code"]);
                _stringBuilder.Append(":");
                _stringBuilder.Append(currency["mid"]);
                _stringBuilder.Append("\n");
            }
        }

        private JToken GetCurrenciesData(string currentExchangeData)
        {
            var exchangeData = JArray.Parse(currentExchangeData);
            return exchangeData.First["rates"];
        }
    }


}
