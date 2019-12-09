using Microsoft.Azure.ServiceBus.Management;
using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class TopicsCommand : Command<TopicOptions>
    {
        public TopicsCommand(IConfigurationStore store, TopicOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            await GetAll(Options.ConnectionName);
        }

        /// <summary>
        /// Display all of the topics in the connection, one per line.
        /// </summary>
        /// <param name="connectionName">The user-given friendly name of the connection.</param>
        public async Task GetAll(string connectionName)
        {
            string connectionString = ConnectionString(connectionName);

            ManagementClient client = new ManagementClient(connectionString);
            IList<TopicDescription> topics = await client.GetTopicsAsync();

            foreach (TopicDescription topic in topics)
            {
                Console.WriteLine(topic.Path);
            }

            await client.CloseAsync();
        }
    }
}
