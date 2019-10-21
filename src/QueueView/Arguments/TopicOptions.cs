using CommandLine;

namespace QueueView.Arguments
{
    [Verb("topics", HelpText = "Operations on topics. See 'topics --help' for more.")]
    public class TopicOptions
    {
        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.", SetName = "All")]
        public string ConnectionName { get; set; }

        [Option('d', "default", HelpText = "Get or set the default topic. Use . to get.", SetName = "Default")]
        public string DefaultTopicName { get; set; }
    }
}
