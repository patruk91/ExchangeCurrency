using System;

namespace ExchangeCurrency.Model
{
    public class StatusCodeException : Exception
    {
        public int CodeNumber { get; set; }
        public StatusCodeException(int codeNumber, string message) : base(message)
        {
            this.CodeNumber = codeNumber;
        }
    }
}
