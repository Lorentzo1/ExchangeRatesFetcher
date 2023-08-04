using HtmlAgilityPack;

namespace ExchangeRateWebApi.Models
{
    public record class DataNode
    {
        public string CountryName { get; init; }
        public string CurrencyName { get; init; }
        public int OneCrown { get; init; }
        public string IsoCode { get; init; }
        public decimal ExchangeRate { get; init; }

        public DataNode(HtmlNode trParent)
        {
            var childNodes = trParent.ChildNodes.Where(x => x.Name == "td").ToList();
            if (childNodes.Count != 5)
            {

                throw new Exception("Data in wrong format");
            }
            if (!int.TryParse(childNodes[2].InnerText, out int crown))
            {
                throw new FormatException($"{nameof(OneCrown)} parse was not succesful");
            }

            if (!decimal.TryParse(childNodes[4].InnerText, out decimal exchangeRate))
            {
                throw new FormatException($"{nameof(ExchangeRate)} parse was not succesful");
            }
            if (crown == 100)
            {
                exchangeRate = exchangeRate / 100;
            }

            CountryName = childNodes[0]?.InnerText!;
            CurrencyName = childNodes[1]?.InnerText!;
            OneCrown = crown;
            IsoCode = childNodes[3]?.InnerText!;
            ExchangeRate = exchangeRate;
            if (CountryName is null || CurrencyName is null || IsoCode is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
