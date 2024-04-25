namespace ExchangeRatesProject.Domain.Models
{
    public class Tbl_Log
    {
        public int? ID { get; set; }
        public DateTime? ISLEM_TARIHI { get; set; }
        public string? KULLANICI_IP { get; set; }
        public string? KUR { get; set; }
        public string? YUZDESEL_DEGISIM { get; set; }
        public string? HATA_FONKSIYON { get; set; }
        public string? HATA_ACIKLAMASI { get; set; }
    }
}
