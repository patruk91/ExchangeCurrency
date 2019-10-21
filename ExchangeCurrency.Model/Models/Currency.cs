using System.Collections.Generic;

namespace ExchangeCurrency.Model.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public List<Conversions> Conversions { get; set; }

        public Currency() { }

        public Currency(string code)
        {
            Code = code;
        }
    }
}