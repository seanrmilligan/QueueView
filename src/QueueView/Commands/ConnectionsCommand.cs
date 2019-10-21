using QueueView.Arguments;
using QueueView.Configuration;
using QueueView.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class ConnectionsCommand : Command<ConnectionOptions>
    {
        public ConnectionsCommand(IConfigurationStore store, ConnectionOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override Task Execute()
        {
            if (!string.IsNullOrEmpty(Options.ConnectionNameUpsert) && !string.IsNullOrEmpty(Options.ConnectionStringUpsert))
            {
                Upsert(Options.ConnectionNameUpsert, Options.ConnectionStringUpsert, Options.Update);
            }
            else if (!string.IsNullOrEmpty(Options.ConnectionNameDelete))
            {
                Delete(Options.ConnectionNameDelete);
            }
            else if (!string.IsNullOrEmpty(Options.DefaultConnectionName))
            {
                // A period is specified as the way of differentiating 'get' from 'set'
                if (Options.DefaultConnectionName.Equals("."))
                {
                    GetDefault();
                }
                else
                {
                    SetDefault(Options.DefaultConnectionName);
                }
            }
            else
            {
                GetAll();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Delete a connection by name.
        /// </summary>
        /// <param name="connectionName">The user-given name of the connection.</param>
        private void Delete(string connectionName)
        {
            Models.Configuration config = Store.ReadConfiguration();
            Connection connection = config.Connection(connectionName);

            if (connection == null)
            {
                Console.WriteLine("No connection with this name was found.");
                return;
            }

            if (config.DefaultConnectionName.Equals(connection.Name))
            {
                config = config.With(defaultConnectionName: string.Empty);
            }

            config.Connections.Remove(connection);

            Store.WriteConfiguration(config);
        }

        /// <summary>
        /// Display all of the connections that have been saved, one per line.
        /// </summary>
        private void GetAll()
        {
            Models.Configuration config = Store.ReadConfiguration();

            // Order the connections for consistency across calls.
            foreach (Connection connection in config.Connections.OrderBy(c => c.Name))
            {
                Console.WriteLine($"{connection.Name} {connection.ConnectionString}");
            }
        }

        /// <summary>
        /// Display the default connection.
        /// </summary>
        private void GetDefault()
        {
            Models.Configuration config = Store.ReadConfiguration();

            Console.WriteLine($"{config.DefaultConnectionName} {config.DefaultConnectionString}");
        }

        /// <summary>
        /// Set the default connection by name.
        /// </summary>
        /// <param name="connectionName">The user-given name of the connection.</param>
        private void SetDefault(string connectionName)
        {
            Models.Configuration config = Store.ReadConfiguration();
            Connection existingConnection = config.Connection(connectionName);

            if (existingConnection == null)
            {
                Console.WriteLine("No connection with this name was found.");
                return;
            }

            config = config.With(defaultConnectionName: connectionName);
            Store.WriteConfiguration(config);
        }

        /// <summary>
        /// Add or update a connection.
        /// If a connection with the <see cref="connectionName"/> does not exist, add the connection.
        /// If a connection with the <see cref="connectionName"/> does exist, do not update unless <see cref="update"/> is true.
        /// </summary>
        /// <param name="connectionName">The user-given name of the connection.</param>
        /// <param name="connectionString">The Azure Service Bus connection string.</param>
        /// <param name="update">Replace the connection if a matching <see cref="connectionName"/> is found.</param>
        private void Upsert(string connectionName, string connectionString, bool update)
        {
            Models.Configuration config = Store.ReadConfiguration();
            Connection existingConnection = config.Connection(connectionName);

            if (existingConnection != null)
            {
                if (update)
                {
                    config.Connections.Remove(existingConnection);
                }
                else
                {
                    Console.WriteLine("A connection with this name already exists. Specify -u to update.");
                    return;
                }
            }

            config.Connections.Add(new Connection(connectionName, connectionString));
            Store.WriteConfiguration(config);
        }
    }
}
