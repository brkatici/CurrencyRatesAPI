using ExchangeRatesProject.Domain.Models;
using static ExchangeRatesProject.Application.Services.CurrencyService;

namespace ExchangeRatesProject.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetCurrencyDataByDate(int year, int month, int day);
        Task<CurrencyDifference[]> CalculateCurrencyDifference(int year, int month, int day);
    }
}
