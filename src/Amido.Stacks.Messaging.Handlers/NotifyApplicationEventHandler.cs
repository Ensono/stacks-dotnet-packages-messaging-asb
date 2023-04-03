using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Messaging.Events;
using Amido.Stacks.Messaging.Handlers.TestDependency;

namespace Amido.Stacks.Messaging.Handlers
{
    public class NotifyApplicationEventHandler : IApplicationEventHandler<NotifyApplicationEvent>
    {
        private readonly ITestable<NotifyApplicationEvent> _testable;

        public NotifyApplicationEventHandler(ITestable<NotifyApplicationEvent> testable)
        {
            _testable = testable;
        }

        public Task HandleAsync(NotifyApplicationEvent applicationEvent)
        {
            _testable.Complete(applicationEvent);
            return Task.CompletedTask;
        }
    }
}