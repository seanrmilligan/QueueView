using CommandLine;

namespace QueueView.Arguments
{
    [Verb("status", HelpText = "Metadata for queues. See 'status --help' for more.")]
    public class StatusOptions
    {
        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.")]
        public string ConnectionName { get; set; }

        [Option('q', "queue", HelpText = "The name of the queue to use.", SetName = "Queue")]
        public string QueueName { get; set; }

        [Option('s', "subscription", HelpText = "The name of the subscription to use.", SetName = "Subscription")]
        public string SubscriptionName { get; set; }

        [Option('t', "topic", HelpText = "The name of the topic to use.", SetName = "Subscription")]
        public string TopicName { get; set; }
    }
}
