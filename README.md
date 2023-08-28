# ExchangeRatesFetcher #

ExchangeRatesFetcher is a small WebApi C# project utilizing web scraping. It can get exchange rate data from [Czech National Bank](https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/)https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/). Exchange rates are in regards to 1 CZK and are possible to get through ExcchangeRate/GetExchangeRates endpoint.

## Used Principals ##
Web scraping is achieved by use of HtmlAgilityPack library
Url for http client is loaded through Option pattern
In-memory cache is used to improve performance and in order to store data in intermediate- layer
Background Service is used to refresh the cache and keeping the data up to date.


