using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageEnvelopeBuilder
    {
        ReceivedMessageEnvelope<T> BuildFrom<T>(Message message) where T : class;
        ServiceBusReceivedMessageEnvelope<T> BuildFrom<T>(ServiceBusReceivedMessage message) where T : class;
    }
}
