using CommandLine;

namespace QueueView.Arguments
{
    [Verb("subscriptions", HelpText = "Operations on subscriptions. See 'subscriptions --help' for more.")]
    public class SubscriptionOptions
    {
        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.", SetName = "All")]
        public string ConnectionName { get; set; }

        [Option('d', "default", HelpText = "Get or set the default subscription. Use . to get.", SetName = "Default")]
        public string DefaultSubscriptionName { get; set; }
        
        [Option('t', "topic", HelpText = "The name of the topic to use if not the default.", SetName = "All")]
        public string TopicName { get; set; }
    }
}
