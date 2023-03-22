using Amido.Stacks.Application.CQRS.ApplicationEvents;

namespace TestCommon
{
    public class NotifyEvent : IApplicationEvent
    {
        public int OperationCode { get; }
        public Guid CorrelationId { get; }
        public int EventCode { get; }

        public NotifyEvent(int operationCode, string correlationId, int eventCode)
        {
            OperationCode = operationCode;
            CorrelationId = Guid.Parse(correlationId);
            EventCode = eventCode;
        }
    }
}
