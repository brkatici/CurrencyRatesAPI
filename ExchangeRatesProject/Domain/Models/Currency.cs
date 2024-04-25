namespace ExchangeRatesProject.Domain.Models
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Tarih_Date")]
    public class CurrencyInfo
    {
        [XmlElement(ElementName = "Currency")]
        public List<Currency> Currencies { get; set; }
    }

    public class Currency
    {
        [XmlAttribute(AttributeName = "CrossOrder")]
        public string CrossOrder { get; set; }
        [XmlAttribute(AttributeName = "Kod")]
        public string Kod { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        public string Unit { get; set; }
        public string Isim { get; set; }
        public string CurrencyName { get; set; }
        public string ForexBuying { get; set; }
        public string ForexSelling { get; set; }
        public string BanknoteBuying { get; set; }
        public string BanknoteSelling { get; set; }
        public string CrossRateUSD { get; set; }
        public string CrossRateOther { get; set; }
    }
}
