﻿using System.Threading.Tasks;

namespace ExchangeCurrency.Model
{
    public interface IExchange
    {
        Task<string> GetCurrentExchangeRates(string uriString, string requestUri);
        string GetCodeCurrencies(string currentExchangeRates);
        string GetExchangeRates(string currentExchangeRates);
    }
}