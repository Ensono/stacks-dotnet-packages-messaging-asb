using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Events
{
    /// <summary>
    /// A wrapper class around an message that contains properties used to set properties on the underlying Message
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.servicebus.message?view=azure-dotnet"/>
    /// </summary>
    /// <typeparam name="T">The type of the underlying event</typeparam>
    public class MessageEnvelope : IHasCorrelationId
    {
        /// <summary>
        /// The session identifier for a session-aware entity
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// The underlying event data
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// The "user properties" bag, which can be used for custom message metadata
        /// </summary>
        public IReadOnlyDictionary<string, object> UserProperties { get; private set; }
        /// <summary>
        /// The content type descriptor
        /// </summary>
        public string ContentType { get; private set; }
        /// <summary>
        /// The a correlation identifier
        /// </summary>
        public string CorrelationId { get; private set; }
        /// <summary>
        /// An application specific label
        /// </summary>
        public string Label { get; private set; }
        /// <summary>
        /// The MessageId to identify the message
        /// </summary>
        public string MessageId { get; private set; }
        /// <summary>
        /// A partition key for sending a message to a partitioned entity
        /// </summary>
        public string PartitionKey { get; private set; }
        /// <summary>
        /// The address of an entity to send replies to
        /// </summary>
        public string ReplyTo { get; private set; }
        /// <summary>
        /// The session identifier augmenting the ReplyTo address
        /// </summary>
        public string ReplyToSessionId { get; private set; }
        /// <summary>
        /// The date and time in UTC at which the message will be enqueued.
        /// This property returns the time in UTC; when setting the property, the supplied DateTime value must also be in UTC
        /// </summary>
        public DateTime ScheduledEnqueueTimeUtc { get; private set; }
        /// <summary>
        /// The message’s "time to live" value.
        /// </summary>
        public TimeSpan? TimeToLive { get; private set; }
        /// <summary>
        /// The "to" address.
        /// </summary>
        public string To { get; private set; }
        /// <summary>
        /// A partition key for sending a message into an entity via a partitioned transfer queue.
        /// </summary>
        public string ViaPartitionKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessageEnvelope(object data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            UserProperties = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
        }

        public MessageEnvelope WithUserProperties(IDictionary<string, object> userProperties)
        {
            UserProperties = new ReadOnlyDictionary<string, object>(userProperties);
            return this;
        }

        public MessageEnvelope WithSessionId(string sessionId)
        {
            SessionId = sessionId;
            return this;
        }

        public MessageEnvelope WithContentType(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public MessageEnvelope WithCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }

        public MessageEnvelope WithLabel(string label)
        {
            Label = label;
            return this;
        }

        public MessageEnvelope WithMessageId(string messageId)
        {
            MessageId = messageId;
            return this;
        }

        public MessageEnvelope WithPartitionKey(string partitionKey)
        {
            PartitionKey = partitionKey;
            return this;
        }

        public MessageEnvelope WithReplyTo(string replyTo)
        {
            ReplyTo = replyTo;
            return this;
        }

        public MessageEnvelope WithReplyToSessionId(string replyToSessionId)
        {
            ReplyToSessionId = replyToSessionId;
            return this;
        }

        public MessageEnvelope WithScheduledEnqueueTimeUtc(DateTime scheduledEnqueueTimeUtc)
        {
            ScheduledEnqueueTimeUtc = scheduledEnqueueTimeUtc;
            return this;
        }

        public MessageEnvelope WithTimeToLive(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            return this;
        }

        public MessageEnvelope WithTo(string to)
        {
            To = to;
            return this;
        }

        public MessageEnvelope WithViaPartitionKey(string viaPartitionKey)
        {
            ViaPartitionKey = viaPartitionKey;
            return this;
        }
    }
}
