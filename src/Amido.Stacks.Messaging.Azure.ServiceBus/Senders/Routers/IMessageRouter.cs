using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Routers
{
    public interface IMessageRouter
    {
        Task SendAsync(object message);
        Task SendAsync(IEnumerable<object> messages);
        bool Match(Type type);
    }

    public interface ITopicRouter : IMessageRouter { }
    public interface IQueueRouter : IMessageRouter { }
}
