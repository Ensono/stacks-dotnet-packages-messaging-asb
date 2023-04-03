using Azure.Messaging.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Extensions
{
    public static class ServiceBusReceivedMessageExtensions
    {
        public static string GetSerializerType(this ServiceBusReceivedMessage message)
        {
            message.ApplicationProperties.TryGetValue($"{MessageProperties.Serializer}", out var value);
            return value as string;
        }

        public static string GetEnclosedMessageType(this ServiceBusReceivedMessage message)
        {
            message.ApplicationProperties.TryGetValue($"{MessageProperties.EnclosedMessageType}", out var value);
            return value as string;
        }
    }
}
