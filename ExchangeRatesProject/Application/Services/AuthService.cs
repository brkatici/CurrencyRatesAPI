using ExchangeRatesProject.Application.Interfaces;
using ExchangeRatesProject.Domain.Interfaces;
using ExchangeRatesProject.Domain.Models;
using ExchangeRatesProject.Infrastructure.Data.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExchangeRatesProject.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly string key;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;


        public AuthService(string key, AppDbContext context,IUserRepository userRepository)
        {

            this._context = context;
            this.key = key;
            this._userRepository = userRepository;



        }
        public async Task<string> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetSpecificUser(email);

            if (user != null)
            {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes(key);
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("UserId", user.Id.ToString())
                        }),
                        //set duration of token here
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature) //setting sha256 algorithm
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return tokenHandler.WriteToken(token);
            }

            // Kullanıcı yok veya şifre doğrulaması başarısız olduysa null döndürülür
            return null;
        }

    }
}
