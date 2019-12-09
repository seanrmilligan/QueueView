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
        /// Constructs an empty configuration.
        /// </summary>
        public Configuration()
        {
            Connections = new List<Connection>();
            DefaultConnectionName = string.Empty;
        }

        /// <summary>
        /// Constructs a configuration with the supplied values.
        /// Used for permuting the configuration via the <see cref="With"/> method.
        /// </summary>
        /// <param name="connections">A list of connection strings with user-given friendly names.</param>
        /// <param name="defaultConnectionName">The user-given friendly name of the default connection.</param>
        [JsonConstructor]
        public Configuration(
            List<Connection> connections,
            string defaultConnectionName
            )
        {
            Connections = connections ?? throw new NullReferenceException(nameof(connections));
            DefaultConnectionName = defaultConnectionName ?? throw new NullReferenceException(nameof(defaultConnectionName));
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
        /// <returns>A new <see cref="Configuration"/> with the specified changes.</returns>
        public Configuration With(
            List<Connection> connections = null,
            string defaultConnectionName = null
            )
        {
            return new Configuration(
                connections ?? Connections,
                defaultConnectionName ?? DefaultConnectionName
                );
        }
    }
}
