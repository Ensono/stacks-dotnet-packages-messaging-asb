using System;
using System.Threading;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Listeners;
using Amido.Stacks.Messaging.Events;
using Amido.Stacks.Messaging.Handlers.TestDependency;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Handlers
{
    /// <summary>
    /// Represents a custom message processor used
    /// </summary>
    public class CustomMessageProcessor : IMessageProcessor
    {
        private readonly ITestable<NotifyApplicationEvent> testable;

        public CustomMessageProcessor(ITestable<NotifyApplicationEvent> testable)
        {
            this.testable = testable;
        }

        public Task ProcessAsync(Message message, CancellationToken cancellationToken)
        {
            //Do what you gotta do, you are on your own now

            NotifyApplicationEvent eventMsg = new NotifyApplicationEvent(Guid.Parse(message.CorrelationId), 098);

            this.testable.Complete(eventMsg);

            return Task.CompletedTask;
        }
    }
}
