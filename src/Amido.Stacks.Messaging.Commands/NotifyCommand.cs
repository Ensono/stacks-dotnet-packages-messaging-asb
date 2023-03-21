using System;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Core.Operations;

namespace Amido.Stacks.Messaging.Commands
{
    public class NotifyCommand : ICommand
    {
        private Guid correlationId;

        public NotifyCommand(Guid correlationId, string testMember)
        {
            OperationCode = 666;
            CorrelationId = correlationId.ToString();
            TestMember = testMember;
        }

        public string TestMember { get; }
        public int OperationCode { get; }

        Guid IOperationContext.CorrelationId => correlationId;

        public string CorrelationId { get; }
    }
}
