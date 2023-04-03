using System.Collections.Generic;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageBuilder
    {
        Message Build<T>(T body);
        Message Build(IMessageEnvelope body);
        IEnumerable<Message> Build<T>(IEnumerable<T> bodies);
        IEnumerable<Message> Build(IEnumerable<IMessageEnvelope> bodies);
    }
}
