using ExchangeRatesProject.Domain.Models;
using ExchangeRatesProject.Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ExchangeRatesProject.Domain.Interfaces;

namespace ExchangeRatesProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> GetSpecificUser(string email)
        {
            // input kontrol
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            // Kullanıcı varsa
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            var adminuser = user;

            // Kullanıcı yoksa hata döndür
            if (user == null)
                throw new ($"User with email '{email}' not found.");

            return user;
        }
    }
}
