using System.Collections.Generic;
using Microsoft.Azure.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Serializers
{
    public interface IMessageBuilder
    {
        Message Build<T>(T body);
        IEnumerable<Message> Build<T>(IEnumerable<T> bodies);
    }
}
