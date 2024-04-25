using ExchangeRatesProject.Application.Services;
using ExchangeRatesProject.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using ExchangeRatesProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ExchangeRatesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // JWT Auth
    public class CurrencyController : Controller
    {

        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }


        [HttpGet("GetByDate")]
        //Belirli bir tarihe göre kur verilerini getiren fonksiyon (EURO, USD, GBP)
        //Tarih aralığı belirtilmez ise güncel kur verilerini çeker
        public async Task<IActionResult> GetCurrencyDataByDate(int year, int month, int day)
        {
            try
            {
                var result = await _currencyService.GetCurrencyDataByDate(year, month, day);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, "Hata oluştu: " + e.Message);
            }
        }

        [HttpGet("CurrencyDifference")]
        //Girilen tarihteki kur verileri ile bugünün güncel kur verilerini fark bazında ve yüzdesel değişim olarak hesaplar
        public async Task<IActionResult> CalculateDifference(int year, int month, int day)
        {
            try
            {
                var result = await _currencyService.CalculateCurrencyDifference(year, month, day);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
