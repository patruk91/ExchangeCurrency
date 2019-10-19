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
        public async Task<string> GetCurrentExchangeRates(string uriString, string requestUri)
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
    }


}
