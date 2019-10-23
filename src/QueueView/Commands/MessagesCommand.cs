using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using QueueView.Arguments;
using QueueView.Configuration;
using QueueView.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class MessagesCommand : Command<MessageOptions>
    {
        public MessagesCommand(IConfigurationStore store, MessageOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            if (!string.IsNullOrEmpty(Options.QueueName))
            {
                await ListMessages(
                    Options.Count,
                    Options.ConnectionName,
                    Options.QueueName,
                    Options.DeadLetter,
                    Options.Columns,
                    Options.Pretty
                    );
            }
            else
            {
                await ListMessages(
                    Options.Count,
                    Options.ConnectionName,
                    Options.TopicName,
                    Options.SubscriptionName,
                    Options.DeadLetter,
                    Options.Columns,
                    Options.Pretty
                    );
            }
        }

        /// <summary>
        /// List messages from a queue.
        /// </summary>
        /// <param name="count">The max number of messages to list.</param>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="queueName">The name of the queue.</param>
        /// <param name="deadLetter">Directs whether this method should read messages from the dead letter sub-queue.</param>
        /// <param name="fields">A list of the specific fields from the message to print, such as 'Body' or 'User Properties'.</param>
        /// <param name="pretty">If true, prints messages in a tabular format and may truncate values.</param>
        /// <returns>nothing</returns>
        public async Task ListMessages(
            uint count,
            string connectionName,
            string queueName,
            bool deadLetter,
            IEnumerable<string> fields,
            bool pretty
            )
        {
            string queuePath = QueuePath(queueName, deadLetter);
            await ListMessages(count, connectionName, queuePath, fields, pretty);
        }

        /// <summary>
        /// List messages from a subscription.
        /// </summary>
        /// <param name="count">The max number of messages to list.</param>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="topicName">The name of the topic under which the subscription can be found.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <param name="deadLetter">Directs whether this method should read messages from the dead letter sub-queue.</param>
        /// <param name="fields">A list of the specific fields from the message to print, such as 'Body' or 'User Properties'.</param>
        /// <param name="pretty">If true, prints messages in a tabular format and may truncate values.</param>
        /// <returns>nothing</returns>
        public async Task ListMessages(
            uint count,
            string connectionName,
            string topicName,
            string subscriptionName,
            bool deadLetter,
            IEnumerable<string> fields,
            bool pretty
            )
        {
            string subscriptionPath = SubscriptionPath(topicName, subscriptionName, deadLetter);
            await ListMessages(count, connectionName, subscriptionPath, fields, pretty);
        }

        /// <summary>
        /// Lists messages from Azure Service Bus located at a given <see cref="entityPath"/>.
        /// </summary>
        /// <param name="count">The max number of messages to list.</param>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="entityPath">The constructed path to the resource where messages are found.</param>
        /// <param name="fields">A list of the specific fields from the message to print, such as 'Body' or 'User Properties'.</param>
        /// <param name="pretty">If true, prints messages in a tabular format and may truncate values.</param>
        /// <returns>nothing</returns>
        private async Task ListMessages(
            uint count,
            string connectionName,
            string entityPath,
            IEnumerable<string> fields,
            bool pretty
            )
        {
            string connectionString = ConnectionString(connectionName);
            MessageReceiver receiver = new MessageReceiver(connectionString, entityPath);
            IMessageFormat formatter;
            uint i = 0;

            if (pretty)
                formatter = new TableFormat(fields.ToList());
            else
                formatter = new CsvFormat(fields.ToList());
            
            Console.WriteLine(formatter.FormatHeaders());

            for (
                Message message = await receiver.PeekAsync();
                message != null;
                message = await receiver.PeekAsync()
            )
            {
                if (i == count) break;
                i++;
                Console.WriteLine(formatter.Format(message));
            }
        }
    }
}
