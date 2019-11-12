using System;

namespace QueueView.Models
{
    /// <summary>
    /// A connection represents a duple of a user-given friendly name and an Azure Service Bus connection string.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// A user-given friendly name for this <see cref="Connection"/>.
        /// </summary>
        /// <example>MyServiceBusConnection</example>
        public readonly string Name;

        /// <summary>
        /// An Azure Service Bus connection string.
        /// </summary>
        /// <example>Endpoint=sb://NAMESPACE.servicebus.windows.net/;SharedAccessKeyName=KEYNAME;SharedAccessKey=KEY</example>
        public readonly string ConnectionString;
        
        /// <summary>
        /// Constructs a new <see cref="Connection"/> given a <see cref="name"/> and <see cref="connectionString"/>.
        /// </summary>
        /// <param name="name">A user-given friendly name for this <see cref="Connection"/>.</param>
        /// <param name="connectionString">An Azure Service Bus connection string.</param>
        public Connection(string name, string connectionString)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
