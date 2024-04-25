using ExchangeRatesProject.Domain.Models;
using ExchangeRatesProject.Infrastructure.Repositories;
using System.Diagnostics;

namespace ExchangeRatesProject.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<Tbl_Log> CreateLogTable(Tbl_Log log);
    }
}
