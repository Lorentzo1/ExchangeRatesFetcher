using ExchangeRateUpdater;

namespace ExchangeRateWebApi.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> MapDataToExchangeRatesAsync();
    }
}
