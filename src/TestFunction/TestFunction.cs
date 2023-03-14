using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestCommon;

namespace TestFunction
{
    public class TestFunction
    {
        private readonly IEventPublisher _eventPublisher;

        public TestFunction(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher ?? throw  new ArgumentNullException(nameof(eventPublisher));
        }

        [FunctionName(nameof(TestFunction))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("This is a test");

            var events = new List<MessageEnvelope>();
            for (var i = 0; i < 100; i++)
            {
                var userProperties = new Dictionary<string, object>
                {
                    { "Prop1", "xyz" },
                    { "Prop2", "123" }
                };

                var messageEnvelope = new MessageEnvelope(new NotifyEvent(i, Guid.NewGuid().ToString(), i))
                        .WithCorrelationId(Guid.NewGuid().ToString())
                        .WithLabel("label")
                        .WithTo("to")
                        .WithUserProperties(userProperties)
                        .WithMessageId("messageId")
                        .WithContentType("contentType")
                        .WithPartitionKey("partitionKey")
                        .WithReplyTo("replyTo")
                        .WithReplyToSessionId("replyToSessionId")
                        .WithSessionId("sessionId")
                        .WithTimeToLive(new TimeSpan(0, 0, 15, 0))
                        .WithViaPartitionKey("viaPartitionKey")
                    ;
                events.Add(messageEnvelope);
            }

            await _eventPublisher.PublishAsync(events);

            //var userProperties = new Dictionary<string, object>
            //{
            //    { "Prop1", "xyz" },
            //    { "Prop2", "123" }
            //};

            //var messageEnvelope = new MessageEnvelope(new NotifyEvent(1, Guid.NewGuid().ToString(), 2))
            //        .WithCorrelationId(Guid.NewGuid().ToString())
            //        .WithLabel("label")
            //        .WithTo("to")
            //        .WithUserProperties(userProperties)
            //        .WithMessageId("messageId")
            //        .WithContentType("contentType")
            //        .WithPartitionKey("partitionKey")
            //        .WithReplyTo("replyTo")
            //        .WithReplyToSessionId("replyToSessionId")
            //        .WithSessionId("sessionId")
            //        .WithTimeToLive(new TimeSpan(0, 0, 15, 0))
            //        .WithViaPartitionKey("viaPartitionKey")
            //    ;

            //await _eventPublisher.PublishAsync(messageEnvelope);
            return new OkResult();
        }
    }
}
