using Amido.Stacks.Messaging.Azure.ServiceBus.Events;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Extensions
{
    public static class EventExtensions
    {
        public static ICorrelationIdSettingStage CreateMessageEnvelope(this Event evt)
        {
            return MessageEnvelope.CreateMessageEnvelope(evt);
        }
    }
}
