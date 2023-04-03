namespace Amido.Stacks.Messaging.Azure.ServiceBus
{
    internal static class Constants
    {
        internal static class Defaults
        {
            public const string CommandSerializer = nameof(Serializers.JsonMessageSerializer);
            public const string EventSerializer = nameof(Serializers.CloudEventMessageSerializer);
        }
    }

    public enum MessageProperties
    {
        EnclosedMessageType,
        Serializer
    }
}
