using System;
using System.Dynamic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExchangeRatesProject.Application.Interfaces;
using ExchangeRatesProject.Domain.Interfaces;
using ExchangeRatesProject.Domain.Models;

namespace ExchangeRatesProject.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _client;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrencyService(HttpClient client,ICurrencyRepository currencyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _currencyRepository = currencyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Girilen tarihteki kur verilerini getir
        // Eğer tarih verisi girilmezse güncel kur verilerini getir
        public async Task<List<Currency>> GetCurrencyDataByDate(int year, int month, int day)
        {
            try
            {
               
                string url = "";
                string formattedMonth = month.ToString("D2"); // Ay bilgisini iki basamaklı formata dönüştür
                string formattedDay = day.ToString("D2"); // Gün bilgisini iki basamaklı formata dönüştür
                if (year == 0 || month == 0 || day == 0)
                {
                    url = "https://www.tcmb.gov.tr/kurlar/today.xml"; // Tarih bilgisi girilmemişse güncel kur verilerini getir
                }
                else
                {
                     DateTime date = new DateTime(year, month, day);

                // Hafta sonu kontrolü
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    throw new InvalidOperationException("Haftasonu piyasalar kapalı olduğundan veri erişimi bulunmamaktadır");
                }
                    url = $"https://www.tcmb.gov.tr/kurlar/{year}{formattedMonth}/{formattedDay}{formattedMonth}{year}.xml"; // Tarih bilgisi varsa url yi düzenle
                }
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string xmlData = await response.Content.ReadAsStringAsync();

                    XmlSerializer serializer = new XmlSerializer(typeof(CurrencyInfo));

                    using (TextReader reader = new StringReader(xmlData))
                    {
                        CurrencyInfo currencyInfo = (CurrencyInfo)serializer.Deserialize(reader);
                        List<Currency> currencies = new List<Currency>();

                        Currency usdCurrency = currencyInfo.Currencies.FirstOrDefault(currency => currency.CurrencyCode == "USD"); // Veriler arasından istenen kur verilerinin elde edilmesi
                        Currency eurCurrency = currencyInfo.Currencies.FirstOrDefault(currency => currency.CurrencyCode == "EUR");
                        Currency gbpCurrency = currencyInfo.Currencies.FirstOrDefault(currency => currency.CurrencyCode == "GBP");

                        currencies.Add(usdCurrency);
                        currencies.Add(eurCurrency);
                        currencies.Add(gbpCurrency);

                        return currencies;
                    }
                }
                else
                {
                    throw new HttpRequestException("HTTP isteği başarısız: " + response.StatusCode);
                }
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException("Hata oluştu: " + e.Message);
            }
        }

        // Girilen tarihteki kur verileri ile bugünün kur verilerini karşılaştır
        // Sonucu Logla
        public async Task<CurrencyDifference[]> CalculateCurrencyDifference(int year, int month, int day)
        {
            try
            {
                var historicalData = await GetCurrencyDataByDate(year, month, day);

                // Bugünkü kur verilerini al
                var todayData = await GetCurrencyDataByDate(0, 0, 0);

                // Farkları hesapla
                var usdDifference = CalculateDifference(todayData[0], historicalData[0]);
                var eurDifference = CalculateDifference(todayData[1], historicalData[1]);
                var gbpDifference = CalculateDifference(todayData[2], historicalData[2]);

                await LogCurrencyDifference(historicalData, todayData, usdDifference, eurDifference, gbpDifference ,year,month,day);
                return new CurrencyDifference[] { usdDifference, eurDifference, gbpDifference };
            }
            catch (Exception e)
            {
                throw new Exception("Hata oluştu: " + e.Message);
            }
        }

        private async Task LogCurrencyDifference(List<Currency> historicalData, List<Currency> todayData, CurrencyDifference usdDifference, 
            CurrencyDifference eurDifference, CurrencyDifference gbpDifference,int year, int month, int day)
        {
            Tbl_Log logEntry = new Tbl_Log
            {
                ISLEM_TARIHI = DateTime.UtcNow,
                KULLANICI_IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
                KUR = $"USD Today {todayData[0].ForexBuying}, USD at {day}/{month}/{year} = {historicalData[0].ForexBuying}, " +
                      $"EUR Today {todayData[1].ForexBuying}, EUR at {day}/{month}/{year} = {historicalData[1].ForexBuying}, " +
                      $"GBP Today {todayData[2].ForexBuying}, GBP at {day}/{month}/{year} = {historicalData[2].ForexBuying}",
                YUZDESEL_DEGISIM = $"EUR: {eurDifference.PercentageDifference}, USD: {usdDifference.PercentageDifference}, GBP: {gbpDifference.PercentageDifference}",

                HATA_ACIKLAMASI = "",
                HATA_FONKSIYON = ""
            };

            await _currencyRepository.CreateLogTable(logEntry);
        }

        public class CurrencyDifference
        {
            public string AmountDifference { get; set; }
            public string PercentageDifference { get; set; }
        }
        private CurrencyDifference CalculateDifference(Currency today, Currency historical)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            double todayForexBuying = double.Parse(today.ForexBuying, culture);
            double historicalForexBuying = double.Parse(historical.ForexBuying, culture);

            double difference = todayForexBuying - historicalForexBuying;
            double percentageDifference = difference / todayForexBuying * 100;

            // Farkı 4 basamaklı formatla
            string formattedDifference = difference.ToString("0.0000", culture);
            // Yüzde kısmını 4 basamaklı formatla
            string formattedPercentageDifference = percentageDifference.ToString("0.0000", culture) + "%";

            return new CurrencyDifference
            {
                AmountDifference = formattedDifference,
                PercentageDifference = formattedPercentageDifference
            };
        }

    }
}
