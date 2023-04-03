namespace Amido.Stacks.Messaging.Azure.ServiceBus.Events
{
    public class Event : IEvent
    {
        public string CorrelationId { get; protected set; }
    }
}
