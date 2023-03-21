using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent eventToPublish);

        /// <summary>
        /// Publish an event wrapped in a MessageEnvelope that allows you to set metadata on the underlying ServiceBus Message object
        /// </summary>
        /// <example>
        /// <code>
        /// var userProperties = new Dictionary&lt;string, object&gt;
        ///   {
        ///      { "Prop1", "xyz" },
        ///      { "Prop2", "123" }
        ///   };
        ///
        /// var messageEnvelope = new MessageEnvelope(new NotifyEvent(i, Guid.NewGuid().ToString(), i))
        ///  .WithCorrelationId(Guid.NewGuid().ToString())
        ///  .WithLabel("label")
        ///  .WithTo("to")
        ///  .WithUserProperties(userProperties)
        ///  .WithMessageId("messageId")
        ///  .WithContentType("contentType")
        ///  .WithPartitionKey("partitionKey")
        ///  .WithReplyTo("replyTo")
        ///  .WithReplyToSessionId("replyToSessionId")
        ///  .WithSessionId("sessionId")
        ///  .WithTimeToLive(new TimeSpan(0, 0, 15, 0))
        ///  .WithViaPartitionKey("viaPartitionKey")
        ///
        ///  await _eventPublisher.PublishAsync(messageEnvelope);
        /// </code>
        /// </example>
        /// <param name="eventToPublish">This method publishes the event set in the Data property of the MessageEnvelope object</param>
        /// <returns></returns>
        Task PublishAsync(IMessageEnvelope eventToPublish);

        Task PublishAsync(IEnumerable<IEvent> eventsToPublish);

        /// <summary>
        /// Bulk publish events wrapped in a MessageEnvelope that allows you to set metadata on the underlying ServiceBus Message object
        /// </summary>
        /// <param name="eventsToPublish">This method publishes the event set in the Data property of the MessageEnvelope object</param>
        /// <returns></returns>
        Task PublishAsync(IEnumerable<IMessageEnvelope> eventsToPublish);
    }
}
