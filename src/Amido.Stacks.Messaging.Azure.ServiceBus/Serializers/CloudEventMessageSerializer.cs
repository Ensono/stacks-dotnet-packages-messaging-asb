using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Amido.Stacks.Core.Operations;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Exceptions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Amido.Stacks.Messaging.Azure.ServiceBus.Serializers
{
    public class CloudEventMessageSerializer : IMessageBuilder, IMessageReader
    {
        private const string DEFAULT_CONTENT_TYPE = "application/cloudevents+json;charset=utf-8";

        public Message Build<T>(T body)
        {
            var message = BuildMessage(body);
            return message.SetSerializerType(GetType());
        }

        public Message Build(IMessageEnvelope body)
        {
            var message = BuildMessage(body);
            return message.SetSerializerType(GetType());
        }

        public IEnumerable<Message> Build<T>(IEnumerable<T> bodies)
        {
            return bodies.Select(Build);
        }

        public IEnumerable<Message> Build(IEnumerable<IMessageEnvelope> bodies)
        {
            return bodies.Select(Build);
        }

        private static Message BuildMessage<T>(T body)
        {
            var correlationId = GetCorrelationId(body);

            var cloudEvent = new StacksCloudEvent<T>(body, correlationId)
            {
                DataContentType = "application/json",
                Source = GetSource()
            };

            var message = new Message
            {
                CorrelationId = $"{correlationId}",
                ContentType = DEFAULT_CONTENT_TYPE,
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cloudEvent))
            };

            message.SetSessionId(body);
            return message.SetEnclosedMessageType(typeof(StacksCloudEvent<>).MakeGenericType(body.GetType()));
        }

        private static Message BuildMessage(IMessageEnvelope envelope)
        {
            var cloudEvent = CreateCloudEvent(envelope.Data.GetType(), envelope.Data, Guid.Parse(envelope.CorrelationId));

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cloudEvent)));

            if (!string.IsNullOrEmpty(envelope.CorrelationId))
            {
                message.CorrelationId = envelope.CorrelationId;
            }

            message.ContentType = !string.IsNullOrEmpty(envelope.ContentType) ? envelope.ContentType : DEFAULT_CONTENT_TYPE;

            if (!string.IsNullOrEmpty(envelope.Label))
            {
                message.Label = envelope.Label;
            }

            if (!string.IsNullOrEmpty(envelope.ReplyTo))
            {
                message.ReplyTo = envelope.ReplyTo;
            }

            if (!string.IsNullOrEmpty(envelope.To))
            {
                message.To = envelope.To;
            }

            if (!string.IsNullOrEmpty(envelope.PartitionKey))
            {
                message.PartitionKey = envelope.PartitionKey;
            }

            if (envelope.ScheduledEnqueueTimeUtc.HasValue)
            {
                message.ScheduledEnqueueTimeUtc = envelope.ScheduledEnqueueTimeUtc.Value;
            }

            if (!string.IsNullOrEmpty(envelope.ReplyToSessionId))
            {
                message.ReplyToSessionId = envelope.ReplyToSessionId;
            }

            if (!string.IsNullOrEmpty(envelope.ViaPartitionKey))
            {
                message.ViaPartitionKey = envelope.ViaPartitionKey;
            }

            if (!string.IsNullOrEmpty(envelope.SessionId))
            {
                message.SessionId = envelope.SessionId;
            }

            if (!string.IsNullOrEmpty(envelope.MessageId))
            {
                message.MessageId = envelope.MessageId;
            }

            if (envelope.TimeToLive.HasValue)
            {
                message.TimeToLive = envelope.TimeToLive.Value;
            }

            foreach (var property in envelope.UserProperties)
            {
                message.UserProperties[property.Key] = envelope.UserProperties[property.Key];
            }

            return message.SetEnclosedMessageType(typeof(StacksCloudEvent<>).MakeGenericType(envelope.Data.GetType()));
        }

        private static object CreateCloudEvent(Type dataType, object data, Guid correlationId)
        {
            var type = typeof(StacksCloudEvent<>).MakeGenericType(dataType);
            var cloudEvent = Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { data, correlationId }, null);

            type.GetMethod("set_DataContentType", BindingFlags.Instance | BindingFlags.Public).Invoke(cloudEvent, new object[] { "application/json" });
            type.GetMethod("set_Source", BindingFlags.Instance | BindingFlags.Public).Invoke(cloudEvent, new object[] { GetSource() });
            return cloudEvent;
        }

        /// <summary>
        /// Read the message body and deserialize the CloudEvent data into the type T
        /// In case the message contains the type enclosed, the serializer will first 
        /// deserialize to the type provided in the message then 
        /// cast the result to the type of T.
        /// This operation will throw an exception if the enclosed message type is not convertible to the type of T.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Read(Message message)
        {
            try
            {
                var cloudEvent = ReadMessageBody<CloudEvent>(message);
                return cloudEvent.Data;
            }
            catch (InvalidCastException ex)
            {
                throw new MessageInvalidCastException("Invalid cast of the object in the message body", message, ex);
            }
        }

        /// <summary>
        /// Read the message body and deserialize into the type T
        /// In case the message contains the type enclosed, the serializer will first 
        /// deserialize to the type provided in the message then 
        /// cast the result to the type of T.
        /// This operation will throw an exception if the enclosed message type is not convertible to the type of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public T ReadMessageBody<T>(Message message)
        {
            var messageType = message.GetEnclosedMessageType();

            var type = GetEnclosedMessageType(messageType);
            if (type == null)
            {
                throw new MessageSerializationException("The message type is unknown", message);
            }

            try
            {
                if (message.Body is null || message.Body.Length == 0)
                {
                    throw new MessageBodyIsNullException($"The body of the message {message.MessageId} is null.", message);
                }

                var obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body), type);

                var cloudEvent = (CloudEvent)obj;
                if (cloudEvent?.SpecVersion != "1.0")
                {
                    throw new MessageSerializationException("Only version 1.0 of the Cloud Events specs is supported.", message);
                }

                return (T)obj;
            }
            catch (InvalidCastException ex)
            {
                throw new MessageInvalidCastException("Invalid cast of the object in the message body", message, ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new MessageSerializationException("Failed to deserialize the message", message, ex);
            }
        }

        private static Type GetEnclosedMessageType(string messageType)
        {
            Type type = null;
            if (!string.IsNullOrEmpty(messageType))
            {
                type = Type.GetType(messageType);
            }

            return type;
        }

        public object Read(ServiceBusReceivedMessage message)
        {
            try
            {
                var cloudEvent = ReadMessageBody<CloudEvent>(message);
                return cloudEvent.Data;
            }
            catch (InvalidCastException ex)
            {
                throw new MessageInvalidCastException("Invalid cast of the object in the message body", message, ex);
            }
        }

        public T ReadMessageBody<T>(ServiceBusReceivedMessage message)
        {
            var messageType = message.GetEnclosedMessageType();

            var type = GetEnclosedMessageType(messageType);
            if (type == null)
            {
                throw new MessageSerializationException("The message type is unknown", message);
            }

            try
            {
                if (message.Body is null || message.Body.ToArray().Length == 0)
                {
                    throw new MessageBodyIsNullException($"The body of the message {message.MessageId} is null.", message);
                }

                var obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body.ToArray()), type);

                var cloudEvent = (CloudEvent)obj;
                if (cloudEvent?.SpecVersion != "1.0")
                {
                    throw new MessageSerializationException("Only version 1.0 of the Cloud Events specs is supported.", message);
                }

                return (T)obj;
            }
            catch (InvalidCastException ex)
            {
                throw new MessageInvalidCastException("Invalid cast of the object in the message body", message, ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new MessageSerializationException("Failed to deserialize the message", message, ex);
            }
        }

        private static Guid GetCorrelationId(object body)
        {
            var ctx = body as IOperationContext;
            return ctx?.CorrelationId ?? Guid.NewGuid();
        }

        private static Uri GetSource()
        {
            return new Uri($"{Environment.GetEnvironmentVariable("version")}/{Environment.MachineName}", UriKind.Relative);
        }
    }
}