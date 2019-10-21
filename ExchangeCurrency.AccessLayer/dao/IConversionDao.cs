using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao
{
    public interface IConversionDao
    {
        Task AddConversions(Conversions conversions, ExchangeDbEntities context);
    }
}