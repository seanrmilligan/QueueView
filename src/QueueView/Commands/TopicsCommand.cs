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
            if (!string.IsNullOrEmpty(Options.DefaultTopicName))
            {
                // A period is specified as the way of differentiating 'get' from 'set'
                if (Options.DefaultTopicName.Equals("."))
                {
                    GetDefault();
                }
                else
                {
                    SetDefault(Options.DefaultTopicName);
                }
            }
            else
            {
                await GetAll(Options.ConnectionName);
            }
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

        /// <summary>
        /// Display the default topic.
        /// </summary>
        private void GetDefault()
        {
            Models.Configuration config = Store.ReadConfiguration();

            Console.WriteLine(config.DefaultTopicName);
        }

        /// <summary>
        /// Set the default topic.
        /// </summary>
        /// <param name="topicName">The name of the topic.</param>
        private void SetDefault(string topicName)
        {
            Models.Configuration config = Store.ReadConfiguration();

            config = config.With(defaultTopicName: topicName);

            Store.WriteConfiguration(config);
        }
    }
}
