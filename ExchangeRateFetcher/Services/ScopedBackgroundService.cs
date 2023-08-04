namespace ExchangeRateWebApi.Services
{
    public class ScopedBackgroundService : BackgroundService
    {
        private readonly IExchangeRateService exchangeRateService;
        private readonly ILogger<ScopedBackgroundService> logger;
        private readonly IServiceProvider serviceProvider;

        public ScopedBackgroundService(IExchangeRateService exchangeRateService, ILogger<ScopedBackgroundService> logger, IServiceProvider serviceProvider)
        {
            this.exchangeRateService = exchangeRateService;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromHours(6));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        IExchangeRateService scopedProcessingService =
                            scope.ServiceProvider.GetRequiredService<IExchangeRateService>();

                        await scopedProcessingService.GetDataAsync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Timed Hosted Service run into exception");
            }
        }


    }
}
