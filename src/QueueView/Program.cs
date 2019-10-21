using CommandLine;
using QueueView.Arguments;
using QueueView.Commands;
using QueueView.Configuration;
using System;
using System.Threading.Tasks;

namespace QueueView
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            IConfigurationStore store = new JsonConfigurationStore();

            try
            {
                await Parser.Default
                    .ParseArguments<ConnectionOptions, MessageOptions, QueueOptions, SendOptions, StatusOptions, SubscriptionOptions, TopicOptions>(args)
                    .MapResult(
                async (ConnectionOptions connectionOptions) => await Run(new ConnectionsCommand(store, connectionOptions)),
                async (MessageOptions messageOptions) => await Run(new MessagesCommand(store, messageOptions)),
                async (QueueOptions queueOptions) => await Run(new QueuesCommand(store, queueOptions)),
                async (SendOptions sendOptions) => await Run(new SendCommand(store, sendOptions)),
                async (StatusOptions statusOptions) => await Run(new StatusCommand(store, statusOptions)),
                async (SubscriptionOptions subscriptionOptions) => await Run(new SubscriptionsCommand(store, subscriptionOptions)),
                async (TopicOptions topicOptions) => await Run(new TopicsCommand(store, topicOptions)),
                async errors =>
                {
                    await Task.CompletedTask;
                    Environment.Exit(-1);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return await Task.FromResult(0);
        }

        private static async Task Run<T>(Command<T> command)
        {
            await command.Execute();
        }
    }
}
