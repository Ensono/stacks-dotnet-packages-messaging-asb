using System;
using System.Linq;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Amido.Stacks.Messaging.Events;
using AutoFixture.Xunit2;
using Microsoft.Azure.ServiceBus;
using Shouldly;
using Xunit;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.UnitTests.Factories
{
    public class MessageEnvelopeBuilderTests
    {
        [Theory, AutoData]
        public void Message_MetaData_Is_Read_Correctly(Message message)
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            const int operationCode = 1;
            var sessionId = $"sessionId{Guid.NewGuid()}";
            var subject = $"subject{Guid.NewGuid()}";

            var notifyEvent = new NotifyApplicationEvent(correlationId, operationCode, sessionId, subject);
            var cloudEventMessageSerializer = new CloudEventMessageSerializer();
            var newMessage = cloudEventMessageSerializer.Build(notifyEvent);
            message.Body = newMessage.Body;
            message.SetEnclosedMessageType(Type.GetType(newMessage.GetEnclosedMessageType()));
            message.SetSerializerType(Type.GetType(newMessage.GetSerializerType()));

            var messageReaderFactory = new MessageReaderFactory(new[] { cloudEventMessageSerializer });
            var messageEnvelopeBuilder = new MessageEnvelopeBuilder(messageReaderFactory);

            // Act
            var messageEnvelope = messageEnvelopeBuilder.BuildFrom<StacksCloudEvent<NotifyApplicationEvent>>(message);

            // Assert
            messageEnvelope.ContentType.ShouldBe(message.ContentType);
            messageEnvelope.CorrelationId.ShouldBe(message.CorrelationId);
            messageEnvelope.Data.ShouldBeOfType(typeof(StacksCloudEvent<NotifyApplicationEvent>));
            messageEnvelope.Data.Data.ShouldBeOfType(typeof(NotifyApplicationEvent));
            messageEnvelope.Data.Data.CorrelationId.ShouldBe(notifyEvent.CorrelationId);
            messageEnvelope.Data.Data.SessionId.ShouldBe(notifyEvent.SessionId);
            messageEnvelope.Data.Data.Subject.ShouldBe(notifyEvent.Subject);
            messageEnvelope.Data.Data.OperationCode.ShouldBe(notifyEvent.OperationCode);
            messageEnvelope.Label.ShouldBe(message.Label);
            messageEnvelope.ReplyToSessionId.ShouldBe(message.ReplyToSessionId);
            messageEnvelope.SessionId.ShouldBe(message.SessionId);
            messageEnvelope.TimeToLive.ShouldBe(message.TimeToLive);
            messageEnvelope.MessageId.ShouldBe(message.MessageId);
            messageEnvelope.PartitionKey.ShouldBe(message.PartitionKey);
            messageEnvelope.ReplyTo.ShouldBe(message.ReplyTo);
            messageEnvelope.ScheduledEnqueueTimeUtc.ShouldBe(message.ScheduledEnqueueTimeUtc);
            messageEnvelope.Size.ShouldBe(message.Size);
            messageEnvelope.SystemProperties.ShouldBeEquivalentTo(message.SystemProperties);
            messageEnvelope.To.ShouldBe(message.To);
            messageEnvelope.UserProperties.ToDictionary(x => x.Key, x => x.Value).ShouldBeEquivalentTo(message.UserProperties);
            messageEnvelope.ViaPartitionKey.ShouldBe(message.ViaPartitionKey);
        }
    }
}
