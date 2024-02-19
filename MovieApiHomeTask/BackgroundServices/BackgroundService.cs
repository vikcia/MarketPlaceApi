using System;
using System.Threading;
using System.Threading.Tasks;
using MarketPlaceApi.CustomException;
using MarketPlaceApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarketPlaceApi.BackgroundServices
{
    public class UnpaidOrdersCleanupWorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnpaidOrdersCleanupWorkerService> _logger;
        private readonly IConfiguration _configuration;

        public UnpaidOrdersCleanupWorkerService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger<UnpaidOrdersCleanupWorkerService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Log(LogLevel.Information, "Old unpayed Orders CleanUp Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Log(LogLevel.Information, "Worker initiated at: {time}", DateTimeOffset.UtcNow);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var ordersRepository = scope.ServiceProvider.GetRequiredService<IMarketRepository>()
                            ?? throw new Exception("Service DbOrdersCleanUpWorkerService not found.");

                        var affectedRows = await ordersRepository.DeleteUnpaidOrdersAfterTimeLimit(DateTime.Now.AddHours(-2));

                        _logger.Log(LogLevel.Information, "Succesfully deleted {rows} rows.", affectedRows);
                    }

                    await Task.Delay(GetWorkerPeriod(), stoppingToken);
                }
                catch (ConfigException ex)
                {
                    await StopAsync(stoppingToken);

                    _logger.Log(LogLevel.Error, "DB cleanup error: {message}", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, "DB cleanup error: {message}", ex.Message);

                    await Task.Delay(60000, stoppingToken);
                }
            }

            _logger.Log(LogLevel.Information, "DB cleanup ended.");
        }

        private int GetWorkerPeriod()
        {
            string timeString = _configuration["DbCleanupPeriodInSeconds"]
                ?? throw new ConfigException("DbCleanup period not found.");

            if (!int.TryParse(timeString, out int result))
                throw new ConfigException("DbCleanup period must contain only numbers.");

            return 1 * result * 1000; // in miliseconds
        }
    }
}
