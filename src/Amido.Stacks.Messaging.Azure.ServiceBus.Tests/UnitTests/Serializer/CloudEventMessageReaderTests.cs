using System;
using Amido.Stacks.Messaging.Azure.ServiceBus.Commands;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Amido.Stacks.Messaging.Commands;
using Amido.Stacks.Messaging.Events;
using Shouldly;
using Xunit;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.UnitTests.Serializer
{
    public class CloudEventMessageReaderTests
    {
        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheCommandWillBeParsed()
        {
            var serializer = new CloudEventMessageSerializer();

            var correlationId = Guid.NewGuid();
            var testMember = Guid.NewGuid().ToString();

            var message = serializer.Build<ICommand>(new NotifyCommand(correlationId, testMember));

            var result = serializer.Read<ICommand>(message) as NotifyCommand;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyCommand));
            result.CorrelationId.ShouldBe(correlationId.ToString());
            result.TestMember.ShouldBe(testMember);
        }

        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheApplicationEventWillBeParsed()
        {
            var serializer = new CloudEventMessageSerializer();

            var correlationId = Guid.NewGuid();
            var message = serializer.Build(new NotifyEvent(correlationId, 321, "session-id"));

            var result = serializer.Read<IEvent>(message) as NotifyEvent;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyEvent));
            result.EventCode.ShouldBe(123);
            result.CorrelationId.ShouldBe(correlationId.ToString());
            result.OperationCode.ShouldBe(321);
            result.SessionId.ShouldBe("session-id");
        }
    }
}
