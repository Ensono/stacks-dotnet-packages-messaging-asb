using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Events
{
    public class ServiceBusReceivedMessageEnvelope<T> where T : class
    {
        /// <summary>
        /// The underlying event data
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// The content type descriptor
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// The a correlation identifier
        /// </summary>
        public string CorrelationId { get; private set; }

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
        /// The message’s "time to live" value.
        /// </summary>
        public TimeSpan? TimeToLive { get; private set; }

        /// <summary>
        /// The "to" address.
        /// </summary>
        public string To { get; private set; }

        /// <summary>
        /// The session identifier for a session-aware entity
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// The date and time in UTC at which the message will be enqueued
        /// </summary>
        public DateTimeOffset? ScheduledEnqueueTime { get; private set; }

        /// <summary>
        /// The dead letter error description for the message.
        /// </summary>
        public string DeadLetterErrorDescription { get; private set; }

        /// <summary>
        /// The dead letter reason for the message.
        /// </summary>
        public string DeadLetterReason { get; private set; }

        /// <summary>
        /// The current delivery count
        /// </summary>
        public int DeliveryCount { get; private set; }

        /// <summary>
        /// The original sequence number of the message
        /// </summary>
        public long EnqueuedSequenceNumber { get; private set; }

        /// <summary>
        /// The date and time of the sent time in UT
        /// </summary>
        public DateTimeOffset? EnqueuedTime { get; private set; }

        /// <summary>
        /// The date and time in UTC at which the message is set to expire
        /// </summary>
        public DateTimeOffset? ExpiresAt { get; private set; }

        /// <summary>
        /// The date and time in UTC until which the message will be locked in the queue/subscription
        /// </summary>
        public DateTimeOffset? LockedUntil { get; private set; }

        /// <summary>
        /// The lock token for the current message
        /// </summary>
        public string LockToken { get; private set; }

        /// <summary>
        /// The unique number assigned to a message by Service Bus
        /// </summary>
        public long SequenceNumber { get; private set; }

        /// <summary>
        /// The state of the message
        /// </summary>
        public ServiceBusMessageState State { get; private set; }

        /// <summary>
        /// An application specific label
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        /// A partition key for sending a message into an entity via a partitioned transfer queue
        /// </summary>
        public string TransactionPartitionKey { get; private set; }

        /// <summary>
        /// The application properties bag, which can be used for custom message metadata
        /// </summary>
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; private set; }

        /// <summary>
        /// The name of the queue or subscription that this message was enqueued on, before it was dead-lettered
        /// </summary>
        public string DeadLetterSource { get; private set; }

        public ServiceBusReceivedMessageEnvelope<T> WithDeadLetterSource(string deadLetterSource)
        {
            DeadLetterSource = deadLetterSource;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithScheduledEnqueueTime(DateTimeOffset scheduledEnqueueTime)
        {
            ScheduledEnqueueTime = scheduledEnqueueTime;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithDeadLetterErrorDescription(string deadLetterErrorDescription)
        {
            DeadLetterErrorDescription = deadLetterErrorDescription;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithDeadLetterReason(string deadLetterReason)
        {
            DeadLetterReason = deadLetterReason;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithDeliveryCount(int deliveryCount)
        {
            DeliveryCount = deliveryCount;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithEnqueuedSequenceNumber(long enqueuedSequenceNumber)
        {
            EnqueuedSequenceNumber = enqueuedSequenceNumber;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithEnqueuedTime(DateTimeOffset enqueuedTime)
        {
            EnqueuedTime = enqueuedTime;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithLockedUntil(DateTimeOffset lockedUntil)
        {
            LockedUntil = lockedUntil;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithLockToken(string lockToken)
        {
            LockToken = lockToken;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithSequenceNumber(long sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithState(ServiceBusMessageState state)
        {
            State = state;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithSubject(string subject)
        {
            Subject = subject;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithApplicationProperties(IReadOnlyDictionary<string, object> applicationProperties)
        {
            ApplicationProperties = applicationProperties;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithTransactionPartitionKey(string transactionPartitionKey)
        {
            TransactionPartitionKey = transactionPartitionKey;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithExpiresAt(ServiceBusReceivedMessage message)
        {
            try
            {
                // If the message has not been received. For example if a new message was created but not yet sent and received.
                // The call below will throw an InvalidOperationException
                ExpiresAt = message.ExpiresAt;
            }
            catch (InvalidOperationException)
            {
            }
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithTimeToLive(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithTo(string to)
        {
            To = to;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithContentType(string contentType)
        {
            ContentType = contentType;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithMessageId(string messageId)
        {
            MessageId = messageId;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithPartitionKey(string partitionKey)
        {
            PartitionKey = partitionKey;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithReplyTo(string replyTo)
        {
            ReplyTo = replyTo;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithSessionId(string sessionId)
        {
            SessionId = sessionId;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope<T> WithReplyToSessionId(string replyToSessionId)
        {
            ReplyToSessionId = replyToSessionId;
            return this;
        }

        public ServiceBusReceivedMessageEnvelope(T data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
