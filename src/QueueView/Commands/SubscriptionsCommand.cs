using Microsoft.Azure.ServiceBus.Management;
using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class SubscriptionsCommand : Command<SubscriptionOptions>
    {
        public SubscriptionsCommand(IConfigurationStore store, SubscriptionOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            if (string.IsNullOrEmpty(Options.TopicName))
            {
                await GetAll(Options.ConnectionName);
            } else
            {
                await GetAll(Options.ConnectionName, Options.TopicName);
            }
        }

        /// <summary>
        /// Display all of the subscriptions in the connection, one per line.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        private async Task GetAll(string connectionName)
        {
            string connectionString = ConnectionString(connectionName);

            ManagementClient manager = new ManagementClient(connectionString);
            IList<TopicDescription> topics = await manager.GetTopicsAsync();
            IList<Task<IList<SubscriptionDescription>>> subscriptionTasks = topics.Select(topic => manager.GetSubscriptionsAsync(topic.Path)).ToList();
            List<SubscriptionDescription> subscriptions = new List<SubscriptionDescription>();

            while (subscriptionTasks.Count > 0)
            {
                Task<IList<SubscriptionDescription>> subscriptionTask = await Task.WhenAny(subscriptionTasks);
                subscriptionTasks.Remove(subscriptionTask);
                subscriptions.AddRange(await subscriptionTask);
            }

            // Order the queues for consistency across calls. 
            foreach (SubscriptionDescription subscription in subscriptions.OrderBy(s => s.TopicPath).ThenBy(s => s.SubscriptionName))
            {
                Console.WriteLine(SubscriptionPath(subscription.TopicPath, subscription.SubscriptionName));
            }

            await manager.CloseAsync();
        }

        /// <summary>
        /// Display all of the subscriptions in the topic, one per line.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <param name="topicName">The name of the topic where the subscription can be found.</param>
        private async Task GetAll(string connectionName, string topicName)
        {
            string connectionString = ConnectionString(connectionName);
            string topicPath = TopicPath(topicName);

            ManagementClient manager = new ManagementClient(connectionString);
            IList<SubscriptionDescription> subscriptions = await manager.GetSubscriptionsAsync(topicPath);

            // Order the queues for consistency across calls. 
            foreach (SubscriptionDescription subscription in subscriptions.OrderBy(s => s.TopicPath).ThenBy(s => s.SubscriptionName))
            {
                Console.WriteLine(subscription.SubscriptionName);
            }

            await manager.CloseAsync();
        }
    }
}
