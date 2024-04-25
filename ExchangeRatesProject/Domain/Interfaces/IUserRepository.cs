using ExchangeRatesProject.Domain.Models;

namespace ExchangeRatesProject.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetSpecificUser(string email);
    }
}
