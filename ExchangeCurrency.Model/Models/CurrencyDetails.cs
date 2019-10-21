using System;
using ExchangeCurrency.Model.Enums;

namespace ExchangeCurrency.Model.Models
{
    public class CurrencyDetails
    {
        public int CurrencyDetailsId { get; set; }
        public TableNames TableName { get; set; }
        public string CurrencyInfo { get; set; }
        public string NoBank { get; set; }
        public DateTime EffectiveDate { get; set; }

        public int CurrencyRef { get; set; }
        public Currency Currency { get; set; }

        public CurrencyDetails() { }

        public CurrencyDetails(TableNames tableName, string currencyInfo, string noBank, DateTime effectiveDate)
        {
            TableName = tableName;
            CurrencyInfo = currencyInfo;
            NoBank = noBank;
            EffectiveDate = effectiveDate;
        }
    }
}