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
    public class MessageEnvelope : IMessageEnvelope, ICorrelationIdSettingStage, IMessageEnvelopeOptionalSettingStage
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
        public DateTime? ScheduledEnqueueTimeUtc { get; private set; }
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

        public static ICorrelationIdSettingStage CreateMessageEnvelope(object data)
        {
            return new MessageEnvelope(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MessageEnvelope(object data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            UserProperties = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
        }

        public IMessageEnvelopeOptionalSettingStage WithUserProperties(IDictionary<string, object> userProperties)
        {
            UserProperties = new ReadOnlyDictionary<string, object>(userProperties);
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithSessionId(string sessionId)
        {
            SessionId = sessionId;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithContentType(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithLabel(string label)
        {
            Label = label;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithMessageId(string messageId)
        {
            MessageId = messageId;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithPartitionKey(string partitionKey)
        {
            PartitionKey = partitionKey;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithReplyTo(string replyTo)
        {
            ReplyTo = replyTo;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithReplyToSessionId(string replyToSessionId)
        {
            ReplyToSessionId = replyToSessionId;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithScheduledEnqueueTimeUtc(DateTime scheduledEnqueueTimeUtc)
        {
            ScheduledEnqueueTimeUtc = scheduledEnqueueTimeUtc;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithTimeToLive(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithTo(string to)
        {
            To = to;
            return this;
        }

        public IMessageEnvelopeOptionalSettingStage WithViaPartitionKey(string viaPartitionKey)
        {
            ViaPartitionKey = viaPartitionKey;
            return this;
        }

        public IMessageEnvelope Build()
        {
            return this;
        }
    }

    public interface ICorrelationIdSettingStage
    {
        IMessageEnvelopeOptionalSettingStage WithCorrelationId(string correlationId);
    }

    public interface IMessageEnvelopeOptionalSettingStage : IMessageBuildingStage
    {
        IMessageEnvelopeOptionalSettingStage WithUserProperties(IDictionary<string, object> userProperties);
        IMessageEnvelopeOptionalSettingStage WithViaPartitionKey(string viaPartitionKey);
        IMessageEnvelopeOptionalSettingStage WithTo(string to);
        IMessageEnvelopeOptionalSettingStage WithTimeToLive(TimeSpan timeToLive);
        IMessageEnvelopeOptionalSettingStage WithScheduledEnqueueTimeUtc(DateTime scheduledEnqueueTimeUtc);
        IMessageEnvelopeOptionalSettingStage WithReplyToSessionId(string replyToSessionId);
        IMessageEnvelopeOptionalSettingStage WithReplyTo(string replyTo);
        IMessageEnvelopeOptionalSettingStage WithPartitionKey(string partitionKey);
        IMessageEnvelopeOptionalSettingStage WithMessageId(string messageId);
        IMessageEnvelopeOptionalSettingStage WithLabel(string label);
        IMessageEnvelopeOptionalSettingStage WithContentType(string contentType);
        IMessageEnvelopeOptionalSettingStage WithSessionId(string sessionId);
    }

    public interface IMessageBuildingStage
    {
        IMessageEnvelope Build();
    }

    public interface IMessageEnvelope
    {
        /// <summary>
        /// The session identifier for a session-aware entity
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// The underlying event data
        /// </summary>
        object Data { get; }

        /// <summary>
        /// The "user properties" bag, which can be used for custom message metadata
        /// </summary>
        IReadOnlyDictionary<string, object> UserProperties { get; }

        /// <summary>
        /// The content type descriptor
        /// </summary>
        string ContentType { get; }
        /// <summary>
        /// The a correlation identifier
        /// </summary>
        string CorrelationId { get; }
        /// <summary>
        /// An application specific label
        /// </summary>
        string Label { get; }
        /// <summary>
        /// The MessageId to identify the message
        /// </summary>
        string MessageId { get; }
        /// <summary>
        /// A partition key for sending a message to a partitioned entity
        /// </summary>
        string PartitionKey { get; }
        /// <summary>
        /// The address of an entity to send replies to
        /// </summary>
        string ReplyTo { get; }
        /// <summary>
        /// The session identifier augmenting the ReplyTo address
        /// </summary>
        string ReplyToSessionId { get; }
        /// <summary>
        /// The date and time in UTC at which the message will be enqueued.
        /// This property returns the time in UTC; when setting the property, the supplied DateTime value must also be in UTC
        /// </summary>
        DateTime? ScheduledEnqueueTimeUtc { get; }
        /// <summary>
        /// The message’s "time to live" value.
        /// </summary>
        TimeSpan? TimeToLive { get; }
        /// <summary>
        /// The "to" address.
        /// </summary>
        string To { get; }
        /// <summary>
        /// A partition key for sending a message into an entity via a partitioned transfer queue.
        /// </summary>
        string ViaPartitionKey { get; }
    }
}
