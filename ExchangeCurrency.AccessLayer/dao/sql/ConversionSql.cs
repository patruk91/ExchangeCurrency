using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao.sql
{
    public class ConversionSql : IConversionDao
    {
        public async Task AddConversions(Conversions conversions, ExchangeDbEntities context)
        {
            context.Conversions.Add(conversions);
            await context.SaveChangesAsync();
        }
    }
}