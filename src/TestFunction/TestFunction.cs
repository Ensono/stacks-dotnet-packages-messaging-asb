using System;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestCommon;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Azure.Messaging.ServiceBus;

namespace TestFunction
{
    public class TestFunction
    {
        private readonly IApplicationEventPublisher _eventPublisher;
        private readonly IMessageMetadataBuilder _messageMetadataBuilder;

        public TestFunction(IApplicationEventPublisher eventPublisher, IMessageMetadataBuilder messageMetadataBuilder)
        {
            _eventPublisher = eventPublisher ?? throw  new ArgumentNullException(nameof(eventPublisher));
            _messageMetadataBuilder = messageMetadataBuilder ?? throw new ArgumentNullException(nameof(messageMetadataBuilder));
        }

        [FunctionName("PublishEvents")]
        public async Task<IActionResult> PublishEvents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("This is a test");

            await PublishSingleEvent();
            //await BulkPublishEvent();

            //await PublishSingleMessageEnvelope();
            //await BulkPublishMessageEnvelope();

            return new OkResult();
        }

        [FunctionName("ReadEvents")]
        public void ReadEvents(
            [ServiceBusTrigger("notification-event", "notification-event", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message)
        {
            var messageMetadata = _messageMetadataBuilder.Build<StacksCloudEvent<NotifyEvent>>(message);
            //var messageMetadata = _messageMetadataBuilder.Build<NotifyEvent>(message);
        }

        //private async Task BulkPublishMessageEnvelope()
        //{
        //    var events = new List<IMessageEnvelope>();
        //    for (var i = 0; i < 100; i++)
        //    {
        //        var userProperties = new Dictionary<string, object>
        //        {
        //            { "Prop1", "xyz" },
        //            { "Prop2", "123" }
        //        };

        //        var messageEnvelope = new NotifyEvent(i, Guid.NewGuid().ToString(), i).CreateMessageEnvelope()
        //                .WithCorrelationId(Guid.NewGuid().ToString())
        //                .WithLabel("label")
        //                .WithTo("to")
        //                .WithUserProperties(userProperties)
        //                .WithMessageId("messageId")
        //                .WithContentType("contentType")
        //                .WithPartitionKey("partitionKey")
        //                .WithReplyTo("replyTo")
        //                .WithReplyToSessionId("replyToSessionId")
        //                .WithSessionId("sessionId")
        //                .WithTimeToLive(new TimeSpan(0, 0, 15, 0))
        //                .WithViaPartitionKey("viaPartitionKey")
        //                .Build()
        //            ;
        //        events.Add(messageEnvelope);
        //    }

        //    //await _eventPublisher.PublishAsync(events);
        //}

        //private async Task BulkPublishEvent()
        //{
        //    var events = new List<NotifyEvent>();
        //    for (var i = 0; i < 100; i++)
        //    {
        //        events.Add(new NotifyEvent(i, Guid.NewGuid().ToString(), i));
        //    }

        //    await _eventPublisher.PublishAsync(events);
        //}

        //private async Task PublishSingleMessageEnvelope()
        //{
        //    var userProperties = new Dictionary<string, object>
        //    {
        //        { "Prop1", "xyz" },
        //        { "Prop2", "123" }
        //    };

        //    var messageEnvelope = new NotifyEvent(1, Guid.NewGuid().ToString(), 1).CreateMessageEnvelope()
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
        //        .Build()
        //        ;

        //    //await _eventPublisher.PublishAsync(messageEnvelope);
        //}

        private async Task PublishSingleEvent()
        {
            await _eventPublisher.PublishAsync(new NotifyEvent(1, Guid.NewGuid().ToString(), 2));
        }
    }
}
