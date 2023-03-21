using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Amido.Stacks.Messaging.Azure.ServiceBus.Listeners;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestCommon;

namespace TestHost.ServiceBusListener
{
    public class NotifyEventServiceBusListener : ServiceBusListenerTemplate<NotifyEvent>, IServiceBusListener
    {
        public NotifyEventServiceBusListener(
            ILogger<NotifyEventServiceBusListener> logger,
            IMessageReceiverClientFactory messageReceiverClientFactory,
            IOptions<ServiceBusSubscriptionListenerConfiguration> configuration,
            IMessagerReaderFactory messageReaderFactory) : base(logger, messageReceiverClientFactory, configuration, messageReaderFactory)
        {
        }

        protected override Task<EventStatus> HandleMessageAsync(NotifyEvent message)
        {
            Logger.LogInformation("Handling message {0}", message.CorrelationId);

            return Task.FromResult(EventStatus.Complete);
        }
    }
}
