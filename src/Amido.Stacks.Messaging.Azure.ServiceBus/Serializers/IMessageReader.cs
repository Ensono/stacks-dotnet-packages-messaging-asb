using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageReader
    {
        T Read<T>(Message message);
        T ReadMessageBody<T>(Message message);
        T Read<T>(ServiceBusReceivedMessage message);
        T ReadMessageBody<T>(ServiceBusReceivedMessage message);
    }
}