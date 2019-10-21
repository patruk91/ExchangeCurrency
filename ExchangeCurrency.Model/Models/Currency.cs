namespace ExchangeCurrency.Model.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public decimal ExchangeRate { get; set; }

        public CurrencyDetails Details { get; set; }

        public Currency() { }

        public Currency(string code, decimal exchangeRate)
        {
            Code = code;
            ExchangeRate = exchangeRate;
        }

    }
}