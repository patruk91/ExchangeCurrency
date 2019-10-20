﻿using System.Runtime.InteropServices;

namespace ExchangeCurrency.Model
{
    public static class ApiBankConfiguration
    {
        public const string UriToNbpApi = @"http://api.nbp.pl";
        public const string UriToExchangeRates = @"api/exchangerates/tables";
        public const string UriToExchangeRate = @"api/exchangerates/rates";

        public static string GetUriLink(string uriString)
        {
            return $"{uriString}/";
        }

        public static string GetRequestUri(string requestUri, string tableName, string currency = "")
        {
            return $"{requestUri}/{tableName}/{currency}";
        }
    }
}