using ExchangeRatesProject.Application.Interfaces;
using ExchangeRatesProject.Domain.Models;
using ExchangeRatesProject.Infrastructure.Data.AppDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser(LoginModel user)
        {

            var token = _authService.Authenticate(user.Email, user.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token.Result);
        }
    }
}
