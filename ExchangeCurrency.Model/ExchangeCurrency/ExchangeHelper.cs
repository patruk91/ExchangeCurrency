using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ExchangeCurrency.Model.ExchangeCurrency
{
    public class ExchangeHelper
    {
        public void AddCodes(JToken currencies, StringBuilder stringBuilder)
        {
            var prefix = "";
            foreach (var currency in currencies)
            {
                stringBuilder.Append(prefix);
                prefix = ",";
                stringBuilder.Append(currency["code"]);
            }
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

        public void AddExchangeRates(JToken currencies, StringBuilder stringBuilder)
        {
            foreach (var currency in currencies)
            {
                stringBuilder.Append(currency["code"]);
                stringBuilder.Append(":");
                stringBuilder.Append(currency["mid"]);
                stringBuilder.Append("\n");
            }
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
            decimal.TryParse(exchangeDataFrom, out var exchangeRateFrom);
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
    }
}