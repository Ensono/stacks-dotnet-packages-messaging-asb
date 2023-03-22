using System;
using System.Threading;
using System.Threading.Tasks;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Listeners
{
    /// <summary>
    /// Template class that listens for messages of type <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">The type of event being read from the service bus</typeparam>
    public abstract class ServiceBusListenerTemplate<T>
    {
        private readonly IMessageReceiverClientFactory _messageReceiverClientFactory;
        private IReceiverClient _subcriptionClient;
        private readonly IOptions<ServiceBusQueueListenerConfiguration> _configuration;
        protected readonly ILogger Logger;
        private readonly IMessagerReaderFactory _messageReaderFactory;

        protected ServiceBusListenerTemplate(
            ILogger logger,
            IMessageReceiverClientFactory messageReceiverClientFactory,
            IOptions<ServiceBusQueueListenerConfiguration> configuration,
            IMessagerReaderFactory messageReaderFactory)
        {
            Logger = logger;
            _messageReceiverClientFactory = messageReceiverClientFactory ?? throw new ArgumentNullException(nameof(messageReceiverClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _messageReaderFactory = messageReaderFactory ?? throw new ArgumentNullException(nameof(messageReaderFactory));
        }

        /// <summary>
        /// Start listening for events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = _configuration.Value.ConcurrencyLevel,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                // We disable autocomplete to avoid releasing the message on failure, waiting the lock timeout to do the job
                AutoComplete = false
            };

            _subcriptionClient = await _messageReceiverClientFactory.CreateReceiverClient(_configuration.Value);
            _subcriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        /// <summary>
        /// Read a message and create a reader to convert it to type <see cref="T"/>
        /// </summary>
        /// <param name="message">The service bus message</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        /// <exception cref="Exception">The exception thrown if no serializer can be found for the message</exception>
        protected virtual async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            var serializerName = message.GetSerializerType() ?? _configuration.Value.Serializer;
            if (string.IsNullOrEmpty(serializerName))
            {
                throw new Exception("No serializer has been identified to parse the message");
            }

            var messageReader = _messageReaderFactory.CreateReader(serializerName);
            if (messageReader == null)
            {
                throw new Exception($"No reader has been found for '{serializerName}'");
            }

            var parsedContent = messageReader.Read(message);
            await UpdateMessageStatus(await HandleMessageAsync((T)parsedContent), message.SystemProperties.LockToken);
        }

        private async Task UpdateMessageStatus(EventStatus eventStatus, string lockToken)
        {
            switch (eventStatus)
            {
                case EventStatus.Complete:
                    await CompleteMessage(lockToken);
                    break;
                case EventStatus.Abandon:
                    await AbandonMessage(lockToken);
                    break;
                case EventStatus.DeadLetter:
                default:
                    await DeadLetterMessage(lockToken);
                    break;
            }
        }

        private async Task CompleteMessage(string lockToken)
        {
            await _subcriptionClient?.CompleteAsync(lockToken);
        }

        private async Task AbandonMessage(string lockToken)
        {
            await _subcriptionClient?.AbandonAsync(lockToken);
        }

        private async Task DeadLetterMessage(string lockToken)
        {
            await _subcriptionClient?.DeadLetterAsync(lockToken);
        }

        /// <summary>
        /// Handle the message as an object of type <see cref="T"/>
        /// </summary>
        /// <param name="message">The message to handle</param>
        /// <returns>The EventStatus <see cref="EventStatus"/></returns>
        protected abstract Task<EventStatus> HandleMessageAsync(T message);

        protected virtual Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Logger.LogError(exceptionReceivedEventArgs.Exception,
                $"Receive message operation failed with message '{exceptionReceivedEventArgs.Exception.Message}'.");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop listening for events
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_subcriptionClient?.IsClosedOrClosing != false)
            {
                Logger.LogInformation($"Listener was already Stopped for entity '{_configuration.Value.Name}'.");
            }

            await _subcriptionClient?.CloseAsync();
        }
    }
}
;