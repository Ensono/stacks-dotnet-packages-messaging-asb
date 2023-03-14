using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders
{
    public interface IMessageSender
    {
        string Alias { get; }
        Task SendAsync<T>(T item);
        Task SendAsync<T>(IEnumerable<T> items);

    }

    public interface ITopicSender : IMessageSender { }

    public interface IQueueSender : IMessageSender { }
}
