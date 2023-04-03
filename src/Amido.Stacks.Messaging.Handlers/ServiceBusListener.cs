using Amido.Stacks.Messaging.Azure.ServiceBus.Listeners;
using System;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Amido.Stacks.Messaging.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using Amido.Stacks.Messaging.Handlers.TestDependency;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Handlers
{
    public class ServiceBusListener : ServiceBusListenerTemplate<NotifyEvent>
    {
        private readonly ITestable<Message> _testable;

        public ServiceBusListener(
            ILogger<ServiceBusListener> logger, 
            IMessageReceiverClientFactory messageReceiverClientFactory, 
            IOptions<ServiceBusQueueListenerConfiguration> configuration, 
            IMessagerReaderFactory messageReaderFactory, 
            ITestable<Message> testable) : base(logger, messageReceiverClientFactory, configuration, messageReaderFactory)
        {
            _testable = testable;
        }

        protected override Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            _testable.Complete(message);
            return Task.CompletedTask;
        }

        protected override Task<EventStatus> HandleMessageAsync(NotifyEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
