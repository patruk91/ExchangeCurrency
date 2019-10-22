using System;
using System.Collections.Generic;
using System.Text;
using ExchangeCurrency.ModelExchangeCurrency.ExchangeCurrency;
using Newtonsoft.Json.Linq;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public class ExchangeHelper
    {
        public string GetCodes(JToken currencies)
        {
            var stringBuilder = new StringBuilder();
            var prefix = "";
            foreach (var currency in currencies)
            {
                stringBuilder.Append(prefix);
                prefix = ",";
                stringBuilder.Append(currency["code"]);
            }

            return stringBuilder.ToString();
        }

        public Dictionary<string, int> AddCodes(string[] strCodes)
        {
            var codes = new Dictionary<string, int>();
            for (var i = 0; i < strCodes.Length; i++)
            {
                codes.Add(strCodes[i], i + 1);
            }

            return codes;
        }

        public string GetExchangeRates(JToken currencies)
        {
            var stringBuilder = new StringBuilder();
            foreach (var currency in currencies)
            {
                stringBuilder.Append(currency["code"]);
                stringBuilder.Append(":");
                stringBuilder.Append(currency["mid"]);
                stringBuilder.Append(" ");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public JToken GetDataForCurrencies(string exchangeRateData)
        {
            var exchangeData = JArray.Parse(exchangeRateData);
            return exchangeData.First["rates"];
        }

        public decimal CalculateRatio(decimal exchangeRateFrom, decimal exchangeRateTo)
        {
            return exchangeRateFrom / exchangeRateTo;
        }

        public decimal CalculateResult(int amount, decimal ratio)
        {
            return Math.Round(ratio * amount, decimals: 2, MidpointRounding.AwayFromZero);
        }

        public decimal GetExchangeRate(JObject currencyData)
        {
            var exchangeDataFrom = currencyData["rates"].First["mid"].ToString();
            Decimal.TryParse(exchangeDataFrom, out var exchangeRateFrom);
            return exchangeRateFrom;
        }

        public DateTime GetDataTransaction(JObject currencyDataFrom)
        {
            var effectiveDate = currencyDataFrom["rates"].First["effectiveDate"] + " " + DateTime.Now.ToString("h:mm:ss");
            return DateTime.Parse(effectiveDate);
        }

        public JObject ParseToJObject(string exchangeRateDataFrom)
        {
            return JObject.Parse(exchangeRateDataFrom); ;
        }

        public Dictionary<string, int> LoadEmptyCodeCurrencies()
        {
            return new Dictionary<string, int>();
        }

        public Dictionary<string, int> LoadCodeCurrencies(IExchange exchange, string uriString, string requestUriAllRates)
        {
            var exchangeData = exchange.GetExchangeRatesData(uriString, requestUriAllRates).Result;
            return exchange.GetCodesForExchangeRates(exchangeData);
        }

        public Dictionary<string, decimal> GetActualRates(string exchangeRates)
        {
            var actualRates = new Dictionary<string, decimal>();
            foreach (var line in exchangeRates.Split(" "))
            {
                var data = line.Split(":");
                var code = data[0];
                decimal.TryParse(data[1], out var value);
                actualRates.Add(code, value);
            }

            return actualRates;
        }
    }
}