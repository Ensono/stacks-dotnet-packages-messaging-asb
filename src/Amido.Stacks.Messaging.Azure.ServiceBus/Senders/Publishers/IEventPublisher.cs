using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers
{
    public interface IEventPublisher
    {
        Task PublishAsync(IHasCorrelationId eventToPublish);

        Task PublishAsync(IEnumerable<IHasCorrelationId> eventsToPublish);
    }
}
