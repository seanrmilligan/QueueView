using CommandLine;

namespace QueueView.Arguments
{
    [Verb("connections", HelpText = "Operations on connections. See 'connections --help' for more.")]
    public class ConnectionOptions
    {
        [Option('s', "connection-string", HelpText = "When adding, the connection string of the new connection. When updating, the replacement connection string.", SetName = "AddOrUpdate")]
        public string ConnectionStringUpsert { get; set; }

        [Option('n', "connection-name", HelpText = "When adding, the friendly name of the connection. When updating, the name of the connection string to replace.", SetName = "AddOrUpdate")]
        public string ConnectionNameUpsert { get; set; }

        [Option('d', "default", HelpText = "Get or set the default connection. Use . to get.", SetName = "Default")]
        public string DefaultConnectionName { get; set; }

        [Option('D', "delete", HelpText = "When deleting a connection, the name of the connection to delete.", SetName = "Delete")]
        public string ConnectionNameDelete { get; set; }
        
        [Option('u', "update", Default = false, HelpText = "Indicates the connection string should be updated for the given connection name.", SetName = "AddOrUpdate")]
        public bool Update { get; set; }
    }
}
