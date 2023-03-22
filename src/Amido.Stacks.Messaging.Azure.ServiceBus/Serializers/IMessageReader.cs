using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageReader
    {
        object Read(Message message);
        T ReadMessageBody<T>(Message message);
        object Read(ServiceBusReceivedMessage message);
        T ReadMessageBody<T>(ServiceBusReceivedMessage message);
    }
}