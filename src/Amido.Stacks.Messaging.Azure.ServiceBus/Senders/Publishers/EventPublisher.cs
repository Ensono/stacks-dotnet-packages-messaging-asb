using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Routers;
using Microsoft.Extensions.Logging;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers
{
    // TODO: This will become MagicRouterLogicToWithCustomStrategies

    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _log;
        private readonly ServiceBusAbstractRouter<ITopicRouter> routing;

        public EventPublisher(
            ILogger<EventPublisher> log,
            ServiceBusAbstractRouter<ITopicRouter> routing
        )
        {
            this._log = log;
            this.routing = routing;
        }

        public async Task PublishAsync(IHasCorrelationId eventToPublish)
        {
            _log.LogInformation($"Publishing event {eventToPublish.CorrelationId}");

            await routing.RouteAsync(eventToPublish);

            _log.LogInformation($"{eventToPublish.CorrelationId}");
        }

        public async Task PublishAsync(IEnumerable<IHasCorrelationId> eventsToPublish)
        {
            _log.LogInformation("Publishing events");

            await routing.RouteAsync(eventsToPublish.ToList());

            _log.LogInformation("Events published");
        }
    }
}
