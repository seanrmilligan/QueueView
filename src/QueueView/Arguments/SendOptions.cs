using CommandLine;

namespace QueueView.Arguments
{
    [Verb("send", HelpText = "Send messages to a topic or queue. See 'send --help' for more.")]
    public class SendOptions
    {
        #region ServiceBus
        ////////////////////////////////////////////////////////////////////////////////
        /// Service Bus
        /// Sources messages to be sent from Azure Service Bus.
        ////////////////////////////////////////////////////////////////////////////////
        [Option('c', "connection", SetName = "ServiceBus", HelpText = "The name of the connection to use while reading messages to send, if not the default.")]
        public string SourceConnectionName { get; set; }

        [Option('q', "queue", SetName = "ServiceBus", HelpText = "The name of the Azure Service Bus queue to read messages from.")]
        public string SourceQueueName { get; set; }

        [Option('t', "topic", SetName = "ServiceBus", HelpText = "The name of the Azure Service Bus topic to read messages from.")]
        public string SourceTopicName { get; set; }

        [Option('s', "subscription", SetName = "ServiceBus", HelpText = "The name of the Azure Service Bus subscription to read messages from.")]
        public string SourceSubscriptionName { get; set; }

        [Option('d', "dead-letter", SetName = "ServiceBus", HelpText = "Read from the dead letter queue instead of the main queue or subscription.", Default = false)]
        public bool SourceDeadLetter { get; set; }

        [Option('D', "dequeue", SetName = "ServiceBus", HelpText = "Dequeue the messages as they are read off of Service Bus and sent to the target topic or queue.", Default = false)]
        public bool ConsumeMessages { get; set; }
        #endregion

        #region StandardInput
        ////////////////////////////////////////////////////////////////////////////////
        /// Standard Input
        /// Sources messages to be sent from Standard Input.
        ////////////////////////////////////////////////////////////////////////////////
        [Option('i', "stdin", SetName = "StandardInput", HelpText = "Read the messages to send as newline-separated values from standard input.")]
        public bool Stdin { get; set; }
        #endregion

        #region File
        ////////////////////////////////////////////////////////////////////////////////
        /// File
        /// Sources messages to be sent from the provided file.
        ////////////////////////////////////////////////////////////////////////////////
        [Option('f', "file", SetName = "File", HelpText = "Read the messages to send as newline-separated values from a file with the provided path.")]
        public string FileName { get; set; }
        #endregion

        [Option('C', "destination-connection", Required = true, HelpText = "The name of the connection to use while sending messages, if not the default.")]
        public string DestinationConnectionName { get; set; }

        [Option('Q', "destination-queue", HelpText = "The name of the Azure Service Bus queue to write messages to.")]
        public string DestinationQueueName { get; set; }

        [Option('T', "destination-topic", HelpText = "The name of the Azure Service Bus topic to write messages to.")]
        public string DestinationTopicName { get; set; }
    }
}
