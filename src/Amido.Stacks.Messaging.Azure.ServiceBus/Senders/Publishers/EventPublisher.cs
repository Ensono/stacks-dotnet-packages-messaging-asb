using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Routers;
using Microsoft.Extensions.Logging;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers
{
    public class EventPublisher : IEventPublisher, IApplicationEventPublisher
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

        public async Task PublishAsync(IEvent eventToPublish)
        {
            _log.LogInformation($"Publishing event {eventToPublish.CorrelationId}");

            await routing.RouteAsync(eventToPublish);

            _log.LogInformation($"{eventToPublish.CorrelationId}");
        }

        public async Task PublishAsync(IMessageEnvelope eventToPublish)
        {
            _log.LogInformation($"Publishing event {eventToPublish.CorrelationId}");

            await routing.RouteAsync(eventToPublish);

            _log.LogInformation($"{eventToPublish.CorrelationId}");
        }

        public async Task PublishAsync(IEnumerable<IEvent> eventsToPublish)
        {
            _log.LogInformation("Publishing events");

            await routing.RouteAsync(eventsToPublish.ToList());

            _log.LogInformation("Events published");
        }

        public async Task PublishAsync(IEnumerable<IMessageEnvelope> eventsToPublish)
        {
            _log.LogInformation("Publishing events");

            await routing.RouteAsync(eventsToPublish.ToList());

            _log.LogInformation("Events published");
        }

        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            _log.LogInformation($"Publishing event {applicationEvent.CorrelationId}");

            await routing.RouteAsync(applicationEvent);

            _log.LogInformation($"{applicationEvent.CorrelationId}");
        }
    }
}
