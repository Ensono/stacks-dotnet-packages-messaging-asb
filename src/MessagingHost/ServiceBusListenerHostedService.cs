using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestHost.ServiceBusListener;

namespace TestHost
{
    public class ServiceBusListenerHostedService : IHostedService
    {
        private readonly ILogger<ServiceBusListenerHostedService> _logger;
        private readonly IServiceBusListener _serviceBusListener;
       
        public ServiceBusListenerHostedService(
            ILogger<ServiceBusListenerHostedService> logger, 
            IServiceBusListener serviceBusListener)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusListener = serviceBusListener ?? throw new ArgumentNullException(nameof(serviceBusListener));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Service Bus Lister Hosted Service");

            await _serviceBusListener.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service Bus Lister Hosted Service");

            await _serviceBusListener.StopAsync(cancellationToken);
        }
    }
}
