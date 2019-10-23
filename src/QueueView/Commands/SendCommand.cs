using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using QueueView.Format;

namespace QueueView.Commands
{
    public class SendCommand : Command<SendOptions>
    {
        private readonly IMessageSender _sender;

        public SendCommand(IConfigurationStore store, SendOptions options) : base(store, options)
        {
            string destinationConnectionString = ConnectionString(Options.DestinationConnectionName);
            string path;

            if (!string.IsNullOrEmpty(Options.DestinationQueueName))
            {
                path = QueuePath(Options.DestinationQueueName);
            }
            else if (!string.IsNullOrEmpty(Options.DestinationTopicName))
            {
                path = TopicPath(Options.DestinationTopicName);
            }
            else
            {
                throw new Exception("You must specify either a queue name or a topic name.");
            }

            _sender = new MessageSender(destinationConnectionString, path);
        }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            if (Options.Stdin)
            {
                using (StreamReader stream = new StreamReader(Console.OpenStandardInput()))
                {
                    await StreamMessages(stream);
                }
            }
            else if (!string.IsNullOrEmpty(Options.FileName))
            {
                using (StreamReader stream = new StreamReader(Options.FileName))
                {
                    await StreamMessages(stream);
                }
            }
            else
            {
                string sourceConnectionString = ConnectionString(Options.SourceConnectionName);
                string path;

                if (!string.IsNullOrEmpty(Options.SourceQueueName))
                {
                    path = QueuePath(Options.SourceQueueName, Options.SourceDeadLetter);
                }
                else if (!string.IsNullOrEmpty(Options.SourceTopicName))
                {
                    path = SubscriptionPath(Options.SourceTopicName, Options.SourceSubscriptionName, Options.SourceDeadLetter);
                }
                else
                {
                    throw new Exception("You must specify either a queue name or a topic name.");
                }

                IMessageReceiver receiver = new MessageReceiver(sourceConnectionString, path);

                if (Options.ConsumeMessages)
                {
                    await ConsumeAndSend(receiver);
                }
                else
                {
                    await PeekAndSend(receiver);
                }
            }
        }

        /// <summary>
        /// Peeks messages from a path in Azure Service Bus using the provided <see cref="IMessageReceiver"/>.
        /// Sends the messages that were peeked to a target path using the <see cref="_sender"/>.
        /// </summary>
        /// <param name="receiver">A receiver opened to a path in Azure Service Bus with a connection string.</param>
        /// <returns>nothing</returns>
        private async Task PeekAndSend(IMessageReceiver receiver)
        {
            for (
                Message message = await receiver.PeekAsync();
                message != null;
                message = await receiver.PeekAsync()
            )
            {
                await Send(receiver, message);
            }
        }

        /// <summary>
        /// Dequeues messages from a path in Azure Service Bus using the provided <see cref="IMessageReceiver"/>.
        /// Sends the messages that were dequeued to a target path using the <see cref="_sender"/>.
        /// </summary>
        /// <param name="receiver">A receiver opened to a path in Azure Service Bus with a connection string.</param>
        /// <returns>nothing</returns>
        private async Task ConsumeAndSend(IMessageReceiver receiver)
        {
            for (
                Message message = await receiver.ReceiveAsync();
                message != null;
                message = await receiver.ReceiveAsync()
            )
            {
                await Send(receiver, message);
            }
        }

        /// <summary>
        /// Sends a message and completes the peeking or dequeueing of that message from the provided <see cref="IMessageReceiver"/>.
        /// </summary>
        /// <param name="receiver">The <see cref="IMessageReceiver"/> that the message was retrieved from.</param>
        /// <param name="message">The message to be sent and completed.</param>
        /// <returns>nothing</returns>
        private async Task Send(IMessageReceiver receiver, Message message)
        {
            Console.WriteLine($"Received message: {MessageFormat.GetBody(message)}");
            await _sender.SendAsync(message.Clone());
            Console.Write("Resubmitted message... ");
            await receiver.CompleteAsync(message.SystemProperties.LockToken);
            Console.WriteLine("Completed message.");
        }

        /// <summary>
        /// Reads message bodies from a stream and sends them to Azure Service Bus.
        /// Uses a <see cref="MessageSender"/> previously constructed with the intended queue or topic path.
        /// </summary>
        /// <param name="stream">The stream that provides the message content.</param>
        /// <returns>nothing</returns>
        private async Task StreamMessages(StreamReader stream)
        {
            string line;

            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                Console.WriteLine($"Sending... {line}");
                await _sender.SendAsync(new Message(Encoding.UTF8.GetBytes(line)));
                Console.WriteLine("Sent!");
            }
        }
    }
}
