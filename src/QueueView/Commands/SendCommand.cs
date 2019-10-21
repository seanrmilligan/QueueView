using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

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

                for (
                    Message message = await receiver.ReceiveAsync();
                    message != null;
                    message = await receiver.ReceiveAsync()
                )
                {
                    Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");
                    await _sender.SendAsync(message.Clone());
                    Console.WriteLine("Resubmitted message.");
                    await receiver.CompleteAsync(message.SystemProperties.LockToken);
                    Console.WriteLine("Completed message.");
                }
            }
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
