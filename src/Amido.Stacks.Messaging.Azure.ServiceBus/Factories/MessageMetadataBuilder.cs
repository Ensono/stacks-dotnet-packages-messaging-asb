using System;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Microsoft.Azure.ServiceBus;
using Azure.Messaging.ServiceBus;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Factories
{
    public class MessageMetadataBuilder : IMessageMetadataBuilder
    {
        private readonly IMessagerReaderFactory _messageReaderFactory;

        public MessageMetadataBuilder(IMessagerReaderFactory messageReaderFactory)
        {
            _messageReaderFactory = messageReaderFactory ?? throw new ArgumentNullException(nameof(messageReaderFactory));
        }

        public MessageMetadata<T> Build<T>(Message message) where T : class
        {
            var serializerName = message.GetSerializerType();
            if (string.IsNullOrEmpty(serializerName))
            {
                throw new Exception("No serializer has been identified to parse the message");
            }

            var messageReader = _messageReaderFactory.CreateReader(serializerName);
            if (messageReader == null)
            {
                throw new Exception($"No reader has been found for '{serializerName}'");
            }

            var parsedContent = messageReader.ReadMessageBody<T>(message);
            return new MessageMetadata<T>(parsedContent)
                .WithContentType(message.ContentType)
                .WithCorrelationId(message.CorrelationId)
                .WithExpiresAtUtc(message)
                .WithLabel(message.Label)
                .WithMessageId(message.MessageId)
                .WithPartitionKey(message.PartitionKey)
                .WithReplyTo(message.ReplyTo)
                .WithReplyToSessionId(message.ReplyToSessionId)
                .WithScheduledEnqueueTimeUtc(message.ScheduledEnqueueTimeUtc)
                .WithSessionId(message.SessionId)
                .WithSize(message.Size)
                .WithSystemProperties(message.SystemProperties)
                .WithTimeToLive(message.TimeToLive)
                .WithTo(message.To)
                .WithUserProperties(message.UserProperties)
                .WithViaPartitionKey(message.ViaPartitionKey)
                ;
        }

        public ServiceBusReceivedMessageMetaData<T> Build<T>(ServiceBusReceivedMessage message) where T : class
        {
            var serializerName = message.GetSerializerType();
            if (string.IsNullOrEmpty(serializerName))
            {
                throw new Exception("No serializer has been identified to parse the message");
            }

            var messageReader = _messageReaderFactory.CreateReader(serializerName);
            if (messageReader == null)
            {
                throw new Exception($"No reader has been found for '{serializerName}'");
            }

            var parsedContent = messageReader.ReadMessageBody<T>(message);
            return new ServiceBusReceivedMessageMetaData<T>(parsedContent)
                .WithApplicationProperties(message.ApplicationProperties)
                .WithContentType(message.ContentType)
                .WithCorrelationId(message.CorrelationId)
                .WithDeadLetterErrorDescription(message.DeadLetterErrorDescription)
                .WithDeadLetterReason(message.DeadLetterReason)
                .WithDeadLetterSource(message.DeadLetterSource)
                .WithDeliveryCount(message.DeliveryCount)
                .WithEnqueuedSequenceNumber(message.EnqueuedSequenceNumber)
                .WithEnqueuedTime(message.EnqueuedTime)
                .WithExpiresAt(message)
                .WithLockedUntil(message.LockedUntil)
                .WithLockToken(message.LockToken)
                .WithMessageId(message.MessageId)
                .WithPartitionKey(message.PartitionKey)
                .WithReplyTo(message.ReplyTo)
                .WithReplyToSessionId(message.ReplyToSessionId)
                .WithScheduledEnqueueTime(message.ScheduledEnqueueTime)
                .WithSequenceNumber(message.SequenceNumber)
                .WithSessionId(message.SessionId)
                .WithState(message.State)
                .WithSubject(message.Subject)
                .WithTimeToLive(message.TimeToLive)
                .WithTo(message.To)
                .WithTransactionPartitionKey(message.TransactionPartitionKey)
                ;
        }
    }
}
