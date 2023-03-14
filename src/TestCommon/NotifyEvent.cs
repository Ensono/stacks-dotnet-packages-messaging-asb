using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers;

namespace TestCommon
{
    public class NotifyEvent : IEvent
    {
        public int OperationCode { get; }
        public string CorrelationId { get; }
        public int EventCode { get; }

        public NotifyEvent(int operationCode, string correlationId, int eventCode)
        {
            OperationCode = operationCode;
            CorrelationId = correlationId;
            EventCode = eventCode;
        }
    }
}
