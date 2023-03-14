using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IServiceProvider provider;
        private readonly Dictionary<Type, Type> handlers;

        public MessageHandlerFactory(
            IServiceProvider provider,
            Dictionary<Type, Type> registeredHandlers
        )
        {
            this.provider = provider;
            this.handlers = registeredHandlers;
        }

        public object CreateHandlerFor(Type type)
        {
            if (handlers.ContainsKey(type))
                return provider.GetRequiredService(handlers[type]);
            else
                return default;
        }
    }
}
