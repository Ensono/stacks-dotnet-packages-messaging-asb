using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public class MessageBuilderFactory : IMessageBuilderFactory
    {
        private readonly IEnumerable<Serializers.IMessageBuilder> builders;

        public MessageBuilderFactory(IEnumerable<Serializers.IMessageBuilder> builders)
        {
            this.builders = builders;
        }

        public Serializers.IMessageBuilder CreateMessageBuilder(string name)
        {
            if (name == null)
                throw new Exception("A serializer name must be provided");

            return builders
                .First(x =>
                    x.GetType().GetTypeNameWithAssembly() == name ||
                    x.GetType().Name == name ||
                    x.GetType().FullName == name);
        }
    }
}