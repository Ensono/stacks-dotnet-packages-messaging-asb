using System.Windows.Input;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Listeners;

namespace Amido.Stacks.Messaging.Azure.ServiceBus
{
    internal static class Constants
    {
        internal static class Defaults
        {
            public const string CommandSerializer = nameof(Serializers.JsonMessageSerializer);
            public const string EventSerializer = nameof(Serializers.CloudEventMessageSerializer);

            public const string QueueListenerMessageProcessor = nameof(ServiceBusListenerMessageProcessor<ICommand>);
            public const string TopicListenerMessageProcessor = nameof(ServiceBusListenerMessageProcessor<IEvent>);
        }
    }

    public enum MessageProperties
    {
        EnclosedMessageType,
        Serializer
    }
}
