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
            if (!string.IsNullOrEmpty(Options.DefaultQueueName))
            {
                // A period is specified as the way of differentiating 'get' from 'set'
                if (Options.DefaultQueueName.Equals("."))
                {
                    GetDefault();
                }
                else
                {
                    SetDefault(Options.DefaultQueueName);
                }
            }
            else
            {
                await GetAll(Options.ConnectionName);
            }
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

        /// <summary>
        /// Display the default queue.
        /// </summary>
        private void GetDefault()
        {
            Models.Configuration config = Store.ReadConfiguration();

            Console.WriteLine(config.DefaultQueueName);
        }

        /// <summary>
        /// Set the default queue.
        /// </summary>
        /// <param name="queueName">The system name of the queue.</param>
        private void SetDefault(string queueName)
        {
            Models.Configuration config = Store.ReadConfiguration();

            config = config.With(defaultQueueName: queueName);

            Store.WriteConfiguration(config);
        }
    }
}
