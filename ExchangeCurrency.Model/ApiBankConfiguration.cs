namespace ExchangeCurrency.Model
{
    public static class ApiBankConfiguration
    {
        public const string UriStringToNbpApi = @"http://api.nbp.pl/";
        public const string RequestUriToGetCurrentExchangeRates = @"api/exchangerates/tables/A/";
        public const string RequestUriToGetExchangeRate = @"api/exchangerates/rates/";
    }
}