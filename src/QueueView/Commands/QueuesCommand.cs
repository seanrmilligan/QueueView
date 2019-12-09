using Microsoft.Azure.ServiceBus.Management;
using QueueView.Arguments;
using QueueView.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueView.Commands
{
    public class QueuesCommand : Command<QueueOptions>
    {
        public QueuesCommand(IConfigurationStore store, QueueOptions options) : base(store, options)
        { }

        /// <inheritdoc cref="Command{T}.Execute" />
        public override async Task Execute()
        {
            await GetAll(Options.ConnectionName);
        }

        /// <summary>
        /// Display all of the queues in the connection, one per line.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use if not the default.</param>
        /// <returns>nothing</returns>
        private async Task GetAll(string connectionName)
        {
            string connectionString = ConnectionString(connectionName);
            
            ManagementClient manager = new ManagementClient(connectionString);
            IList<QueueDescription> queues = await manager.GetQueuesAsync();

            // Order the queues for consistency across calls. 
            foreach (QueueDescription queue in queues.OrderBy(q => q.Path))
            {
                Console.WriteLine(queue.Path);
            }

            await manager.CloseAsync();
        }
    }
}
