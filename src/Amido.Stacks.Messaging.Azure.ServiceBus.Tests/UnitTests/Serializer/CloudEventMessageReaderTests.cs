using System;
using Amido.Stacks.Application.CQRS.Commands;
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

            var result = serializer.Read(message) as NotifyCommand;

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
            var message = serializer.Build(new NotifyApplicationEvent(correlationId, 321, "session-id"));

            var result = serializer.Read(message) as NotifyApplicationEvent;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyApplicationEvent));
            result.EventCode.ShouldBe(123);
            result.CorrelationId.ShouldBe(correlationId);
            result.OperationCode.ShouldBe(321);
            result.SessionId.ShouldBe("session-id");
        }
    }
}
