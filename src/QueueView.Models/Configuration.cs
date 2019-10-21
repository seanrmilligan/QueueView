using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace QueueView.Models
{
    public class Configuration
    {
        /// <summary>
        /// A list of connection strings with user-given friendly names.
        /// </summary>
        public readonly List<Connection> Connections;

        /// <summary>
        /// The user-given friendly name of the default connection.
        /// </summary>
        /// <example>MyServiceBusConnection</example>
        public readonly string DefaultConnectionName;
        
        /// <summary>
        /// Get the default Azure Service Bus connection string.
        /// This searches for a <see cref="Connection"/> with the <see cref="DefaultConnectionName"/>.
        /// This is a case-sensitive search.
        /// </summary>
        /// <returns>An Azure Service Bus connection string if there is a match, null otherwise.</returns>
        [JsonIgnore]
        public string DefaultConnectionString => ConnectionString(DefaultConnectionName);

        /// <summary>
        /// The name of a queue in Azure Service Bus. Represents a portion of the path.
        /// The default queue name is used when a queue name is not explicitly provided.
        /// </summary>
        public readonly string DefaultQueueName;

        /// <summary>
        /// The name of a subscription in Azure Service Bus. Represents a portion of the path.
        /// The default subscription name is used when a subscription name is not explicitly provided.
        /// </summary>
        public readonly string DefaultSubscriptionName;

        /// <summary>
        /// The name of a topic in Azure Service Bus. Represents a portion of the path.
        /// The default topic name is used when a topic name is not explicitly provided.
        /// </summary>
        public readonly string DefaultTopicName;
        
        /// <summary>
        /// Constructs an empty configuration.
        /// </summary>
        public Configuration()
        {
            Connections = new List<Connection>();
            DefaultConnectionName = string.Empty;
            DefaultQueueName = string.Empty;
            DefaultSubscriptionName = string.Empty;
            DefaultTopicName = string.Empty;
        }

        /// <summary>
        /// Constructs a configuration with the supplied values.
        /// Used for permuting the configuration via the <see cref="With"/> method.
        /// </summary>
        /// <param name="connections">A list of connection strings with user-given friendly names.</param>
        /// <param name="defaultConnectionName">The user-given friendly name of the default connection.</param>
        /// <param name="defaultQueueName">The name of a queue in Azure Service Bus.</param>
        /// <param name="defaultSubscriptionName">The name of a subscription in Azure Service Bus.</param>
        /// <param name="defaultTopicName">The name of a topic in Azure Service Bus.</param>
        [JsonConstructor]
        public Configuration(
            List<Connection> connections,
            string defaultConnectionName,
            string defaultQueueName,
            string defaultSubscriptionName,
            string defaultTopicName
            )
        {
            Connections = connections ?? throw new NullReferenceException(nameof(connections));
            DefaultConnectionName = defaultConnectionName ?? throw new NullReferenceException(nameof(defaultConnectionName));
            DefaultQueueName = defaultQueueName ?? throw new NullReferenceException(nameof(defaultQueueName));
            DefaultSubscriptionName = defaultSubscriptionName ?? throw new NullReferenceException(nameof(defaultSubscriptionName));
            DefaultTopicName = defaultTopicName ?? throw new NullReferenceException(nameof(defaultTopicName));
        }

        /// <summary>
        /// Get a <see cref="Connection"/> from the configuration given a <see cref="connectionName"/>.
        /// This is a case-sensitive search.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <returns>A <see cref="Connection"/> if there is a match, null otherwise.</returns>
        public Connection Connection(string connectionName) => Connections.FirstOrDefault(c => c.Name.Equals(connectionName));

        /// <summary>
        /// Get an Azure Service Bus connection string given a <see cref="connectionName"/>.
        /// This is a case-sensitive search.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        /// <returns>An Azure Service Bus connection string if there is a match, null otherwise.</returns>
        public string ConnectionString(string connectionName) => Connection(connectionName)?.ConnectionString;

        /// <summary>
        /// Constructs a new <see cref="Configuration"/> with the specified permutations.
        /// </summary>
        /// <param name="connections">A list of connection strings with user-given friendly names.</param>
        /// <param name="defaultConnectionName">The new <see cref="DefaultConnectionName"/>.</param>
        /// <param name="defaultQueueName">The new <see cref="DefaultQueueName"/>.</param>
        /// <param name="defaultSubscriptionName">The new <see cref="DefaultSubscriptionName"/>.</param>
        /// <param name="defaultTopicName">The new <see cref="DefaultTopicName"/>.</param>
        /// <returns>A new <see cref="Configuration"/> with the specified changes.</returns>
        public Configuration With(
            List<Connection> connections = null,
            string defaultConnectionName = null,
            string defaultQueueName = null,
            string defaultSubscriptionName = null,
            string defaultTopicName = null)
        {
            return new Configuration(
                connections ?? Connections,
                defaultConnectionName ?? DefaultConnectionName,
                defaultQueueName ?? DefaultQueueName,
                defaultSubscriptionName ?? DefaultSubscriptionName,
                defaultTopicName ?? DefaultTopicName
                );
        }
    }
}
