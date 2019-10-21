using CommandLine;
using System.Collections.Generic;

namespace QueueView.Arguments
{
    [Verb("messages", HelpText = "Operations on messages. See 'messages --help' for more.")]
    public class MessageOptions
    {
        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.")]
        public string ConnectionName { get; set; }

        [Option('d', "dead-letter", Default = false, HelpText = "Show the dead letter queue messages rather than the queue messages.")]
        public bool DeadLetter { get; set; }

        [Option('f', "fields", Separator = ',', HelpText = "The list of message fields to print. If not specified, all fields are printed.")]
        public IEnumerable<string> Columns { get; set; }

        [Option('n', "count", Default = (uint) 10, HelpText = "The number of messages to show.")]
        public uint Count { get; set; }
        
        [Option('p', "pretty", Default = false, HelpText = "Print the messages in a table format. Values may be truncated to fit the column widths.")]
        public bool Pretty { get; set; }

        [Option('q', "queue", Required = true, HelpText = "The name of the queue to use.", SetName = "Queue")]
        public string QueueName { get; set; }

        [Option('s', "subscription", Required = true, HelpText = "The name of the subscription to use.", SetName = "Subscription")]
        public string SubscriptionName { get; set; }

        [Option('t', "topic", HelpText = "The name of the topic to use.", SetName = "Subscription")]
        public string TopicName { get; set; }
    }
}
