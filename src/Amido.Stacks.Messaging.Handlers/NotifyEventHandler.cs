using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Events;
using Amido.Stacks.Messaging.Handlers.TestDependency;

namespace Amido.Stacks.Messaging.Handlers
{
    public class NotifyEventHandler : IEventHandler<NotifyEvent>
    {
        private readonly ITestable<NotifyEvent> _testable;

        public NotifyEventHandler(ITestable<NotifyEvent> testable)
        {
            _testable = testable;
        }

        public Task HandleAsync(NotifyEvent applicationEvent)
        {
            _testable.Complete(applicationEvent);
            return Task.CompletedTask;
        }
    }
}