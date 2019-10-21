using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao.sql
{
    public class CurrencySql : ICurrencyDao
    {
        public async Task AddCurrency(Currency currency, ExchangeDbEntities context)
        {
            context.Currency.Add(currency);
            await context.SaveChangesAsync();
        }
    }
}