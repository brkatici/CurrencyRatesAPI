using ExchangeRatesProject.Domain.Interfaces;
using ExchangeRatesProject.Domain.Models;
using ExchangeRatesProject.Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExchangeRatesProject.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _appDbContext;

        public CurrencyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
                
        }

        public async Task<Tbl_Log> CreateLogTable(Tbl_Log log)
        {
            await _appDbContext.Logs.AddAsync(log);
            await _appDbContext.SaveChangesAsync();
            return log;
        }



    }
}
