using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestHost
{
    public class ServiceBusReceiverService : BackgroundService
    {
        private readonly ILogger<ServiceBusReceiverService> _logger;

        public ServiceBusReceiverService(ILogger<ServiceBusReceiverService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
