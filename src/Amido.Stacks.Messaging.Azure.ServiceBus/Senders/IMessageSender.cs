using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders
{
    public interface IMessageSender
    {
        string Alias { get; }
        Task SendAsync<T>(T item);
        Task SendAsync(IMessageEnvelope item);
        Task SendAsync<T>(IEnumerable<T> items);
        Task SendAsync(IEnumerable<IMessageEnvelope> items);
    }

    public interface ITopicSender : IMessageSender { }

    public interface IQueueSender : IMessageSender { }
}
