using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessageMetadataBuilder
    {
        MessageMetadata<T> Build<T>(Message message) where T : class;
        MessageMetadata<T> Build<T>(ServiceBusReceivedMessage message) where T : class;
    }
}
