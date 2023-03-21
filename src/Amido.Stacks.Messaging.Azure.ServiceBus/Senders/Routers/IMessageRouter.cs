using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Routers
{
    public interface IMessageRouter
    {
        Task SendAsync(object message);
        Task SendAsync(IMessageEnvelope message);
        Task SendAsync(IEnumerable<object> messages);
        Task SendAsync(IEnumerable<IMessageEnvelope> messages);
        bool Match(Type type);
    }

    public interface ITopicRouter : IMessageRouter { }
    public interface IQueueRouter : IMessageRouter { }
}
