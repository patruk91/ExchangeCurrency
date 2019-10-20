using System.Text;
using Newtonsoft.Json.Linq;

namespace ExchangeCurrency.Model
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

        public decimal GetAverageExchangeRate(string exchangeRateData)
        {
            var currency = JObject.Parse(exchangeRateData);
            var exchangeData = currency["rates"].First["mid"].ToString();
            decimal.TryParse(exchangeData, out var value);
            return value;
        }
    }
}