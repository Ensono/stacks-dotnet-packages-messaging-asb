﻿using System;
using System.Text;
using Amido.Stacks.Messaging.Azure.ServiceBus.Exceptions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Amido.Stacks.Messaging.Commands;
using Amido.Stacks.Messaging.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.UnitTests.Serializer
{
    public class JsonMessageReaderTests
    {
        [Fact]
        public void GivenTheAssemblyNameIsIncorrectAndCannotSerialiseItThrows()
        {
            var parser = new JsonMessageSerializer();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyCommand(new Guid("C8A14B73F2A14696BEEFBD432AF27296"), "669851d4-9836-4e1c-9787-bd914705c4dc")))
            };

            message.SetEnclosedMessageType(typeof(JsonMessageReaderTests));

            ShouldThrowExtensions.ShouldThrow<MessageParsingException>(() => parser.Read(message));
        }

        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheCommandWillBeParsed()
        {
            var parser = new JsonMessageSerializer();
            var correlationId = Guid.NewGuid();
            var testMember = Guid.NewGuid().ToString();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyCommand(correlationId, testMember)))
            };

            message.SetEnclosedMessageType(typeof(NotifyCommand));

            var result = parser.Read(message) as NotifyCommand;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyCommand));
            result.CorrelationId.ShouldBe(correlationId.ToString());
            result.TestMember.ShouldBe(testMember);
        }

        [Fact]
        public void GivenTheParametersCorrectTheBodyOfTheApplicationEventWillBeParsed()
        {
            var parser = new JsonMessageSerializer();
            var correlationId = Guid.NewGuid();
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NotifyApplicationEvent(correlationId, 321, "session-id")))
            };

            message.SetEnclosedMessageType(typeof(NotifyApplicationEvent));

            var result = parser.Read(message) as NotifyApplicationEvent;

            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotifyApplicationEvent));
            result.CorrelationId.ShouldBe(correlationId);
            result.SessionId.ShouldBe("session-id");
        }
    }
}
