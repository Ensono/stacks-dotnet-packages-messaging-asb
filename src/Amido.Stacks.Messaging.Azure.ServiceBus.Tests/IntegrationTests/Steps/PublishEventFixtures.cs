using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders;
using Amido.Stacks.Messaging.Azure.ServiceBus.Senders.Publishers;
using Amido.Stacks.Messaging.Events;
using Amido.Stacks.Messaging.Handlers;
using Amido.Stacks.Messaging.Handlers.TestDependency;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Tests.IntegrationTests.Steps
{
    public class PublishEventFixtures
    {
        private readonly ServiceProvider _provider;
        private readonly ITestable<NotifyApplicationEvent> _testable1;
        private readonly ITestable<NotifyEvent> _testable2;
        private readonly ITestable<Message> _testable3;

        private Guid _correlationId;
        private string _messageId;

        public PublishEventFixtures()
        {
            var assemblies = new[] { typeof(NotifyApplicationEvent).Assembly, typeof(NotifyApplicationEventHandler).Assembly };
            _testable1 = Substitute.For<ITestable<NotifyApplicationEvent>>();
            _testable2 = Substitute.For<ITestable<NotifyEvent>>();
            _testable3 = Substitute.For<ITestable<Message>>();

            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.integration.topics.json")
                .Build();

            var services = new ServiceCollection()
                .AddLogging()
                .AddSecrets()
                .AddTransient<IApplicationEventHandler<NotifyApplicationEvent>, NotifyApplicationEventHandler>()
                .AddTransient<IEventHandler<NotifyEvent>, NotifyEventHandler>()
                .AddTransient(_ => _testable1)
                .AddTransient(_ => _testable2)
                .AddTransient(_ => _testable3)
                .AddTransient(typeof(ILogger<>), typeof(Logger<>))
                .Configure<ServiceBusConfiguration>(configurationRoot.GetSection("ServiceBus"))
                .AddServiceBus()
                ;

            _correlationId = Guid.NewGuid();
            _provider = services.BuildServiceProvider();
        }

        public async Task TheTopicSenderHealthCheckPass()
        {
            var topics = _provider.GetServices<ITopicSender>();
            foreach (var topic in topics)
            {
                var check = await ((IHealthCheck)topic).CheckHealthAsync(null);
                check.Status.Should().Be(HealthStatus.Healthy);
            }
        }

        public void TheCorrectApplicationEventIsSentToTheTopic()
        {
            var eventPublisher = _provider.GetService<IApplicationEventPublisher>();
            eventPublisher.PublishAsync(new NotifyApplicationEvent(_correlationId, 321, "resourceId")).GetAwaiter().GetResult();
        }

        public void TheCorrectEventIsSentToTheTopic()
        {
            var eventPublisher = _provider.GetService<IEventPublisher>();
            eventPublisher.PublishAsync(new NotifyEvent(_correlationId, 321, "resourceId")).GetAwaiter().GetResult();
        }

        public void TheCorrectBatchOfEventsIsSentToTheTopic()
        {
            var eventPublisher = _provider.GetService<IEventPublisher>();
            eventPublisher
                .PublishAsync(Enumerable.Range(1, 5).Select(x => new NotifyEvent(_correlationId, 321, x.ToString())))
                .GetAwaiter().GetResult();
        }

        public void TheCorrectMessageEnvelopeIsSentToTheTopic()
        {
            var eventPublisher = _provider.GetService<IEventPublisher>();

            _messageId = Guid.NewGuid().ToString();
            var messageEnvelope = (new NotifyEvent(_correlationId, 321, "sessionId"))
                .CreateMessageEnvelope()
                .WithCorrelationId(_correlationId.ToString())
                .WithMessageId(_messageId).Build();

            eventPublisher
                .PublishAsync(messageEnvelope)
                .GetAwaiter().GetResult();
        }

        public void TheHostIsRunning()
        {
            var hostService = _provider.GetService<IHostedService>();
            hostService.StartAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public void WaitFor3Seconds()
        {
            const int delay = 3;
            Task.Delay(TimeSpan.FromSeconds(delay)).GetAwaiter().GetResult();
        }

        public void TheApplicationEventIsHandledInTheHandler()
        {
            _testable1.Received(1)
                .Complete(Arg.Is<NotifyApplicationEvent>(applicationEvent => applicationEvent.OperationCode == 321
                                                                  && applicationEvent.CorrelationId == _correlationId));
        }

        public void TheEventIsHandledInTheHandler()
        {
            _testable2.Received(1)
                .Complete(Arg.Is<NotifyEvent>(applicationEvent => applicationEvent.OperationCode == 321
                                                                             && applicationEvent.CorrelationId == _correlationId.ToString()));
        }

        public void TheMessageIsHandledInTheHandler()
        {
            _testable3.Received(1)
                .Complete(Arg.Is<Message>(message => message.MessageId == _messageId
                && message.CorrelationId == _correlationId.ToString()));
        }

        public void TheBatchOfEventsIsHandledInTheHandler()
        {
            _testable2.Received(5)
                .Complete(Arg.Any<NotifyEvent>());
        }

        public void ReadMessageEnvelopeFromTopic()
        {
            var config = _provider
                .GetService<IOptions<ServiceBusConfiguration>>()
                .Value;

            if (config.Listener.Topics.Length > 1)
            {
                throw new Exception("This test is only valid when there is one topic configured");
            }

            var serviceBusListener = new ServiceBusListener(
                _provider.GetRequiredService<ILogger<ServiceBusListener>>(),
                _provider.GetRequiredService<IMessageReceiverClientFactory>(),
               Options.Create(config.Listener.Topics[0]), 
                _provider.GetRequiredService<IMessagerReaderFactory>(), 
                _testable3);

            serviceBusListener.StartAsync(CancellationToken.None).GetAwaiter().GetResult();

        }
    }
}
