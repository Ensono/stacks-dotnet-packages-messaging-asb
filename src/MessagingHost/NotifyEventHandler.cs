using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Microsoft.Extensions.Logging;
using TestCommon;

namespace TestHost
{
    public class NotifyEventHandler : IEventHandler<NotifyEvent>
    {
        private readonly ILogger<NotifyEventHandler> _logger;

        public NotifyEventHandler(ILogger<NotifyEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(NotifyEvent applicationEvent)
        {
            _logger.LogInformation($"Handling event with correlation id: {applicationEvent.CorrelationId}");

            return Task.CompletedTask;
        }
    }
}
