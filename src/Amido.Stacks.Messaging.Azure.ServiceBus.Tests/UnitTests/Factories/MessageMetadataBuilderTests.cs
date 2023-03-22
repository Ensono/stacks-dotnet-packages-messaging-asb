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
    public class MessageMetadataBuilderTests
    {
        [Theory, AutoData]
        public void Message_MetaData_Is_Read_Correctly(Message message)
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            const int operationCode = 1;
            var sessionId = $"sessionId{Guid.NewGuid()}";
            var subject = $"subject{Guid.NewGuid()}";

            var notifyEvent = new NotifyEvent(correlationId, operationCode, sessionId, subject);
            var cloudEventMessageSerializer = new CloudEventMessageSerializer();
            var newMessage = cloudEventMessageSerializer.Build(notifyEvent);
            message.Body = newMessage.Body;
            message.SetEnclosedMessageType(Type.GetType(newMessage.GetEnclosedMessageType()));
            message.SetSerializerType(Type.GetType(newMessage.GetSerializerType()));

            var messageReaderFactory = new MessageReaderFactory(new[] { cloudEventMessageSerializer });
            var messageMetaDataBuilder = new MessageMetadataBuilder(messageReaderFactory);

            // Act
            var messageMetaData = messageMetaDataBuilder.Build<StacksCloudEvent<NotifyEvent>>(message);

            // Assert
            messageMetaData.ContentType.ShouldBe(message.ContentType);
            messageMetaData.CorrelationId.ShouldBe(message.CorrelationId);
            messageMetaData.Data.ShouldBeOfType(typeof(StacksCloudEvent<NotifyEvent>));
            messageMetaData.Data.Data.ShouldBeOfType(typeof(NotifyEvent));
            messageMetaData.Data.Data.CorrelationId.ShouldBe(notifyEvent.CorrelationId);
            messageMetaData.Data.Data.SessionId.ShouldBe(notifyEvent.SessionId);
            messageMetaData.Data.Data.Subject.ShouldBe(notifyEvent.Subject);
            messageMetaData.Data.Data.OperationCode.ShouldBe(notifyEvent.OperationCode);
            messageMetaData.Label.ShouldBe(message.Label);
            messageMetaData.ReplyToSessionId.ShouldBe(message.ReplyToSessionId);
            messageMetaData.SessionId.ShouldBe(message.SessionId);
            messageMetaData.TimeToLive.ShouldBe(message.TimeToLive);
            messageMetaData.MessageId.ShouldBe(message.MessageId);
            messageMetaData.PartitionKey.ShouldBe(message.PartitionKey);
            messageMetaData.ReplyTo.ShouldBe(message.ReplyTo);
            messageMetaData.ScheduledEnqueueTimeUtc.ShouldBe(message.ScheduledEnqueueTimeUtc);
            messageMetaData.Size.ShouldBe(message.Size);
            messageMetaData.SystemProperties.ShouldBeEquivalentTo(message.SystemProperties);
            messageMetaData.To.ShouldBe(message.To);
            messageMetaData.UserProperties.ToDictionary(x => x.Key, x => x.Value).ShouldBeEquivalentTo(message.UserProperties);
            messageMetaData.ViaPartitionKey.ShouldBe(message.ViaPartitionKey);
        }
    }
}
