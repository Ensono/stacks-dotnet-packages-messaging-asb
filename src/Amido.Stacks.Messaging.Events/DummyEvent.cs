using System;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers;

namespace Amido.Stacks.Messaging.Events
{
    public class DummyEvent : IEvent
    {
        public DummyEvent() { }

        public DummyEvent(Guid correlationId)
        {
            this.CorrelationId = correlationId.ToString();
        }

        public int EventCode => 9871;

        public int OperationCode { get; }

        public string CorrelationId { get; }
    }
}
