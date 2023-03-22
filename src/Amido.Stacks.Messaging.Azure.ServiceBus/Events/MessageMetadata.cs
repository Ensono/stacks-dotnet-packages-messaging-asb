using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Events
{
    public class MessageMetadata<T> where T : class
    {
        /// <summary>
        /// The underlying event data
        /// </summary>
        public T Data { get; protected set; }

        /// <summary>
        /// The content type descriptor
        /// </summary>
        public string ContentType { get; protected set; }

        /// <summary>
        /// The a correlation identifier
        /// </summary>
        public string CorrelationId { get; protected set; }

        /// <summary>
        /// The MessageId to identify the message
        /// </summary>
        public string MessageId { get; protected set; }

        /// <summary>
        /// A partition key for sending a message to a partitioned entity
        /// </summary>
        public string PartitionKey { get; protected set; }

        /// <summary>
        /// The address of an entity to send replies to
        /// </summary>
        public string ReplyTo { get; protected set; }

        /// <summary>
        /// The session identifier augmenting the ReplyTo address
        /// </summary>
        public string ReplyToSessionId { get; protected set; }

        /// <summary>
        /// The message’s "time to live" value.
        /// </summary>
        public TimeSpan? TimeToLive { get; protected set; }

        /// <summary>
        /// The "to" address.
        /// </summary>
        public string To { get; protected set; }

        /// <summary>
        /// The session identifier for a session-aware entity
        /// </summary>
        public string SessionId { get; protected set; }

        /// <summary>
        /// The "user properties" bag, which can be used for custom message metadata
        /// </summary>
        public IReadOnlyDictionary<string, object> UserProperties { get; private set; }

        /// <summary>
        /// An application specific label
        /// </summary>
        public string Label { get; private set; }
        
        /// <summary>
        /// The date and time in UTC at which the message will be enqueued.
        /// This property returns the time in UTC; when setting the property, the supplied DateTime value must also be in UTC
        /// </summary>
        public DateTime? ScheduledEnqueueTimeUtc { get; private set; }
        
        /// <summary>
        /// A partition key for sending a message into an entity via a partitioned transfer queue.
        /// </summary>
        public string ViaPartitionKey { get; private set; }

        /// <summary>
        /// The total size of the message body in bytes
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// Properties that are set by the system.
        /// </summary>
        public Message.SystemPropertiesCollection SystemProperties { get; private set; }

        /// <summary>
        /// The date and time in UTC at which the message is set to expire
        /// </summary>
        public DateTime? ExpiresAtUtc { get; private set; }

        public MessageMetadata<T> WithSize(long size)
        {
            Size = size;
            return this;
        }

        public MessageMetadata<T> WithExpiresAtUtc(Message message)
        {
            try
            {
                // If the message has not been received. For example if a new message was created but not yet sent and received.
                // The call below will throw an InvalidOperationException
                ExpiresAtUtc = message.ExpiresAtUtc;
            }
            catch (InvalidOperationException)
            {
            }
            return this;
        }

        public MessageMetadata<T> WithSystemProperties(Message.SystemPropertiesCollection systemProperties)
        {
            SystemProperties = systemProperties;
            return this;
        }

        public MessageMetadata<T> WithUserProperties(IDictionary<string, object> userProperties)
        {
            UserProperties = new ReadOnlyDictionary<string, object>(userProperties);
            return this;
        }

        public MessageMetadata<T> WithLabel(string label)
        {
            Label = label;
            return this;
        }

        public MessageMetadata<T> WithScheduledEnqueueTimeUtc(DateTime scheduledEnqueueTimeUtc)
        {
            ScheduledEnqueueTimeUtc = scheduledEnqueueTimeUtc;
            return this;
        }

        public MessageMetadata<T> WithTimeToLive(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            return this;
        }

        public MessageMetadata<T> WithTo(string to)
        {
            To = to;
            return this;
        }

        public MessageMetadata<T> WithViaPartitionKey(string viaPartitionKey)
        {
            ViaPartitionKey = viaPartitionKey;
            return this;
        }

        public MessageMetadata<T> WithContentType(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public MessageMetadata<T> WithCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }

        public MessageMetadata<T> WithMessageId(string messageId)
        {
            MessageId = messageId;
            return this;
        }

        public MessageMetadata<T> WithPartitionKey(string partitionKey)
        {
            PartitionKey = partitionKey;
            return this;
        }

        public MessageMetadata<T> WithReplyTo(string replyTo)
        {
            ReplyTo = replyTo;
            return this;
        }

        public MessageMetadata<T> WithSessionId(string sessionId)
        {
            SessionId = sessionId;
            return this;
        }

        public MessageMetadata<T> WithReplyToSessionId(string replyToSessionId)
        {
            ReplyToSessionId = replyToSessionId;
            return this;
        }

        public MessageMetadata(T data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
