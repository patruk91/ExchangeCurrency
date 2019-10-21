using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExchangeCurrency.Model.Models;

namespace ExchangeCurrency.AccessLayer.dao
{
    public interface ICurrencyDao
    {
        Task AddCurrency(Currency currency, ExchangeDbEntities context);
    }
}