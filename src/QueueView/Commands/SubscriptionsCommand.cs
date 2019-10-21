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
            if (!string.IsNullOrEmpty(Options.DefaultSubscriptionName))
            {
                // A period is specified as the way of differentiating 'get' from 'set'
                if (Options.DefaultSubscriptionName.Equals("."))
                {
                    GetDefault();
                }
                else
                {
                    SetDefault(Options.DefaultSubscriptionName);
                }
            }
            else
            {
                await GetAll(Options.ConnectionName, Options.TopicName);
            }
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

        /// <summary>
        /// Display the default subscription.
        /// </summary>
        private void GetDefault()
        {
            Models.Configuration config = Store.ReadConfiguration();

            Console.WriteLine(config.DefaultSubscriptionName);
        }

        /// <summary>
        /// Set the default subscription.
        /// </summary>
        /// <param name="subscriptionName">The name of the subscription.</param>
        private void SetDefault(string subscriptionName)
        {
            Models.Configuration config = Store.ReadConfiguration();

            config = config.With(defaultSubscriptionName: subscriptionName);

            Store.WriteConfiguration(config);
        }
    }
}
