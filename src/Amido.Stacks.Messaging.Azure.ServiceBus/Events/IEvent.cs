namespace Amido.Stacks.Messaging.Azure.ServiceBus.Events
{
    public interface IEvent
    {
        string CorrelationId { get; }
    }
}
