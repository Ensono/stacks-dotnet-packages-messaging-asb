using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Routers
{
    /// <summary>
    /// A simple router used by default when a single entity is configured and no routes configured
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultMessageRouter<T> : ITopicRouter, IQueueRouter
        where T : IMessageSender
    {
        private readonly IEnumerable<T> senders;

        public DefaultMessageRouter(
            IEnumerable<T> senders
        )
        {
            this.senders = senders;

            if (senders.Count() != 1)
                throw new Exception("Unable do use default router with multiple senders. Please create a custom routing");
        }

        public async Task SendAsync(object message)
        {
            var sender = senders.First();
            await sender.SendAsync(message);
        }

        public async Task SendAsync(IMessageEnvelope message)
        {
            var sender = senders.First();
            await sender.SendAsync(message);
        }

        public async Task SendAsync(IEnumerable<object> messages)
        {
            var sender = senders.First();
            await sender.SendAsync(messages);
        }

        public async Task SendAsync(IEnumerable<IMessageEnvelope> messages)
        {
            var sender = senders.First();
            await sender.SendAsync(messages);
        }

        public bool Match(Type type) => true;
    }
}
