namespace ExchangeCurrency.ModelExchangeCurrency
{
    public class ApiConnections
    {
        public string UriString { get; set; }   
        public string RequestUriAllRates { get; set; }
        public string RequestUriSingleRate {private get; set; }

        public string RequestUriToSingleRate(string currency)
        {
            return RequestUriSingleRate + currency;
        }
    }
}