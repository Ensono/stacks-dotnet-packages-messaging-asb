using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public interface IMessagerReaderFactory
    {
        IMessageReader CreateReader(string name = null);
    }
}
