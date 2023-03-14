using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Events;
using Shouldly;
using Xunit;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.UnitTests
{
    public class MessageEnvelopeTests
    {
        [Fact]
        public void Properties_Are_Mapped_Correctly()
        {
            var sessionId = $"sessionId{Guid.NewGuid()}";
            var correlationId = Guid.NewGuid();
            var subject = $"subject{Guid.NewGuid()}";
            var notifyEvent = new NotifyEvent(correlationId, 1, sessionId, subject);
            var label = $"label{Guid.NewGuid()}";
            var to = $"to{Guid.NewGuid()}";
            var contentType = $"contentType{Guid.NewGuid()}";
            var messageId = $"messageId{Guid.NewGuid()}";
            var scheduledEnqueueTimeUtc = DateTime.UtcNow;
            var partitionKey = $"partitionKey{Guid.NewGuid()}";
            var replyTo = $"replyTo{Guid.NewGuid()}";
            var replyToSessionId = $"replyToSessionId{Guid.NewGuid()}";
            var timeToLive = new TimeSpan(0, 2, 30, 0);
            var viaPartitionKey = $"viaPartitionKey{Guid.NewGuid()}";
            var userProperty1 = new { Key = "userProperty1Key", Value = "userProperty1" };
            var userProperty2 = new { Key = "userProperty2Key", Value = "userProperty2" };
            var userProperties = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
            {
                { userProperty1.Key, userProperty1.Value },
                { userProperty2.Key, userProperty2.Value }
            });

            // Arrange
            var sut = new MessageEnvelope(notifyEvent);
            sut.WithCorrelationId(correlationId.ToString());
            sut.WithLabel(label);
            sut.WithTo(to);
            sut.WithContentType(contentType);
            sut.WithMessageId(messageId);
            sut.WithScheduledEnqueueTimeUtc(scheduledEnqueueTimeUtc);
            sut.WithPartitionKey(partitionKey);
            sut.WithReplyTo(replyTo);
            sut.WithSessionId(sessionId);
            sut.WithReplyToSessionId(replyToSessionId);
            sut.WithTimeToLive(timeToLive);
            sut.WithMessageId(messageId);
            sut.WithViaPartitionKey(viaPartitionKey);
            sut.WithUserProperties(userProperties);

            // Act

            // Assert
            sut.ContentType.ShouldBe(contentType);
            sut.MessageId.ShouldBe(messageId);
            sut.CorrelationId.ShouldBe(correlationId.ToString());
            sut.Label.ShouldBe(label);
            sut.To.ShouldBe(to);
            sut.PartitionKey.ShouldBe(partitionKey);
            sut.ReplyToSessionId.ShouldBe(replyToSessionId);
            sut.ReplyTo.ShouldBe(replyTo);
            sut.SessionId.ShouldBe(sessionId);
            sut.ViaPartitionKey.ShouldBe(viaPartitionKey);
            sut.TimeToLive.ShouldBe(timeToLive);
            sut.ScheduledEnqueueTimeUtc.ShouldBe(scheduledEnqueueTimeUtc);
            sut.UserProperties.ShouldBeEquivalentTo(userProperties);
        }
    }
}
