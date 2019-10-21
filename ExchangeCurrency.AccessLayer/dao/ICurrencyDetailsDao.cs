using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao
{
    public interface ICurrencyDetailsDao
    {
        Task AddCurrencyDetails(CurrencyDetails currencyDetails, ExchangeDbEntities context);
    }
}