using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;
using ExchangeRateWebApi.Models;
using ExchangeRateWebApi.Utilities;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateWebApi.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly AddressOptions options;
        private readonly ILogger<ExchangeRateService> logger;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;

        public ExchangeRateService(IOptions<AddressOptions> options, ILogger<ExchangeRateService> logger, HttpClient httpClient, IMemoryCache memoryCache)
        {
            this.options = options.Value;
            this.logger = logger;
            this.httpClient = httpClient;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRateAsync()
        {
            try
            {
                var data = await GetDataAsync(options.Url);
                List<ExchangeRate> exchangeRates = data.Select(x => new ExchangeRate(new Currency("CZK"), new Currency(x.IsoCode), x.ExchangeRate)).ToList();
                return exchangeRates;
            }
            catch (Exception ex)
            {
                logger.LogError("Error at MapDataToExchangeRatesAsync", ex);
                throw new Exception(ex.Message);
            }

        }
        private async Task<List<DataNode>> GetDataAsync(string url)
        {
            try
            {

                if (!memoryCache.TryGetValue(CacheKeys.Entry, out List<DataNode>? cacheValue))
                {
                    var date = await httpClient.GetAsync(url);
                    string content = await date.Content.ReadAsStringAsync();

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(content);
                    var first = htmlDoc.DocumentNode.SelectNodes("//div/table/tbody").First();
                    var dataNodes = first.ChildNodes.Where(x => x.Name == "tr").ToList();
                    cacheValue = dataNodes.Select(x => new DataNode(x)).ToList();

                    //cache
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(3));
                    cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                    cacheEntryOptions.SetSize(1024);

                    memoryCache.Set(CacheKeys.Entry, cacheValue, cacheEntryOptions);
                    return cacheValue;

                }
                else
                {
                    var cachedValue = memoryCache.Get(CacheKeys.Entry);
                    return (List<DataNode>)cachedValue!;
                }

            }
            catch (Exception ex)
            {

                logger.LogError("Error at GetDataAsync", ex);
                throw new Exception(ex.Message);
            }
        }

    }
}



