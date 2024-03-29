﻿using System;
using System.Threading.Tasks;
using QueueView.Configuration;
using QueueView.Models;

namespace QueueView.Commands
{
    public abstract class Command<T>
    {
        protected readonly IConfigurationStore Store;
        protected readonly T Options;

        /// <summary>
        /// Constructs a new abstract command.
        /// The command has access to a store of configuration information, and
        /// the options selected for the concrete command extending it.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="options"></param>
        protected Command(IConfigurationStore store, T options)
        {
            Store = store;
            Options = options;
        }

        /// <summary>
        /// Run the command.
        /// Make sense of the options provided and feed them into the helper appropriate methods.
        /// </summary>
        /// <returns>nothing</returns>
        public abstract Task Execute();

        /// <summary>
        /// Resolve an Azure Service Bus connection string from the given <see cref="connectionName"/>.
        /// Throws exceptions if the given <see cref="connectionName"/> is null or empty,
        /// if the <see cref="Connection"/> corresponding to the connectionName cannot be found, or
        /// if the connection string in the <see cref="Connection"/> is null or empty.
        /// </summary>
        /// <param name="connectionName">A user-given friendly name for the saved connection.</param>
        /// <returns>An Azure Service Bus connection string.</returns>
        public string ConnectionString(string connectionName)
        {
            Models.Configuration config = Store.ReadConfiguration();
            string selectedConnectionName = connectionName ?? config.DefaultConnectionName;

            if (string.IsNullOrEmpty(selectedConnectionName))
            {
                throw new Exception("Connection name cannot be null or empty.");
            }

            Connection connection = config.Connection(selectedConnectionName);

            if (connection == null)
            {
                throw new Exception($"No connection found with connection name '{connectionName}'.");
            }

            string connectionString = connection.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string cannot be null or empty.");
            }

            return connectionString;
        }

        /// <summary>
        /// Construct the Azure Service Bus path for a queue.
        /// Throws an <see cref="Exception"/> if the queue name is null or empty.
        /// </summary>
        /// <param name="queueName">The name of the queue.</param>
        /// <param name="deadletter">Indicates whether to construct a path for the main queue or the dead letter sub-queue.</param>
        /// <returns>A queue path understood by Azure Service Bus.</returns>
        public string QueuePath(string queueName, bool deadletter = false)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new Exception("Queue name cannot be null or empty.");
            }

            string queuePath = queueName + (deadletter ? "/$DeadLetterQueue" : string.Empty);

            return queuePath;
        }

        /// <summary>
        /// Construct the Azure Service Bus path for a subscription.
        /// Throws an <see cref="Exception"/> if the topic name or subscription name is null or empty.
        /// </summary>
        /// <param name="topicName">The name of the topic where the subscription can be found.</param>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <param name="deadletter">Indicates whether to construct a path for the main subscription or the dead letter sub-queue.</param>
        /// <returns>A subscription path understood by Azure Service Bus.</returns>
        public string SubscriptionPath(string topicName, string subscriptionName, bool deadletter = false)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new Exception("Topic name cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(subscriptionName))
            {
                throw new Exception("Subscription name cannot be null or empty.");
            }

            string subscriptionPath = $"{topicName}/Subscriptions/{subscriptionName}" + (deadletter ? "/$DeadLetterQueue" : string.Empty);

            return subscriptionPath;
        }

        /// <summary>
        /// Construct the Azure Service Bus path for a topic.
        /// Throws an <see cref="Exception"/> if the topic name is null or empty.
        /// </summary>
        /// <param name="topicName">The name of the topic.</param>
        /// <returns>A topic path understood by Azure Service Bus.</returns>
        public string TopicPath(string topicName)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new Exception("Topic name cannot be null or empty.");
            }

            return topicName;
        }
    }
}
