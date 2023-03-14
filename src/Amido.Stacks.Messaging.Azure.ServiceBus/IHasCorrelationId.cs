namespace Amido.Stacks.Messaging.Azure.ServiceBus
{
    public interface IHasCorrelationId
    {
        string CorrelationId { get; }
    }
}
