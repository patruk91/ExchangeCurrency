using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao.sql
{
    public class CurrencyDetailsSql : ICurrencyDetailsDao
    {
        public async Task AddCurrencyDetails(CurrencyDetails currencyDetails, ExchangeDbEntities context)
        {
            context.CurrencyDetails.Add(currencyDetails);
            await context.SaveChangesAsync();
        }
    }
}