using ExchangeRatesProject.Domain.Models;

namespace ExchangeRatesProject.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(string mail, string password);
    }
}
