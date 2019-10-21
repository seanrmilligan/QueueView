using Microsoft.Azure.ServiceBus.Management;
using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class StatusCommand : Command<StatusOptions>
    {
        public StatusCommand(IConfigurationStore store, StatusOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            if (!string.IsNullOrEmpty(Options.QueueName))
            {
                await GetQueue(Options.ConnectionName, Options.QueueName);
            }
            else
            {
                await GetSubscription(Options.ConnectionName, Options.TopicName, Options.SubscriptionName);
            }
        }

        /// <summary>
        /// Display metadata about a queue.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="queueName">The name of the queue.</param>
        /// <returns>nothing</returns>
        private async Task GetQueue(string connectionName, string queueName)
        {
            string connectionString = ConnectionString(connectionName);
            string queuePath = QueuePath(queueName);

            ManagementClient manager = new ManagementClient(connectionString);
            QueueRuntimeInfo queue = await manager.GetQueueRuntimeInfoAsync(queuePath);

            PrintMessageCountDetails(
                queuePath,
                queue.MessageCount,
                queue.MessageCountDetails);

            await manager.CloseAsync();
        }

        /// <summary>
        /// Display metadata about a subscription.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="topicName">The name of the topic where the subscription can be found.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        public async Task GetSubscription(string connectionName, string topicName, string subscriptionName)
        {
            string connectionString = ConnectionString(connectionName);
            string topicPath = TopicPath(topicName);
            string subscriptionPath = SubscriptionPath(topicName, subscriptionName);

            ManagementClient manager = new ManagementClient(connectionString);
            SubscriptionRuntimeInfo subscription = await manager.GetSubscriptionRuntimeInfoAsync(topicPath, subscriptionName);
            
            PrintMessageCountDetails(
                subscriptionPath,
                subscription.MessageCount,
                subscription.MessageCountDetails);

            await manager.CloseAsync();
        }

        /// <summary>
        /// Print the details for the associated queue or subscription path.
        /// </summary>
        /// <param name="path">The Azure Service Bus path for the queue or subscription.</param>
        /// <param name="messageCount">The total message count.</param>
        /// <param name="details">The message count details for the queue or subscription.</param>
        public void PrintMessageCountDetails(string path, long messageCount, MessageCountDetails details)
        {
            Console.WriteLine("{0,-35}{1,-45}", "Path", path);
            Console.WriteLine("{0,-35}{1,-45}", "Message Count", messageCount);
            Console.WriteLine("{0,-35}{1,-45}", "Active Messages", details.ActiveMessageCount);
            Console.WriteLine("{0,-35}{1,-45}", "DeadLetter Messages", details.DeadLetterMessageCount);
            Console.WriteLine("{0,-35}{1,-45}", "Scheduled Messages", details.ScheduledMessageCount);
            Console.WriteLine("{0,-35}{1,-45}", "Transferred Messages", details.TransferMessageCount);
            Console.WriteLine("{0,-35}{1,-45}", "Transferred DeadLetter Messages", details.TransferDeadLetterMessageCount);
        }
    }
}
