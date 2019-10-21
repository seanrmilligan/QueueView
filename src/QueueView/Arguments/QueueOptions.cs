﻿using CommandLine;

namespace QueueView.Arguments
{
    [Verb("queues", HelpText = "Operations on queues. See 'queues --help' for more.")]
    public class QueueOptions
    {
        [Option('d', "default", HelpText = "Get or set the default queue. Use . to get.", SetName = "Default")]
        public string DefaultQueueName { get; set; }

        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.", SetName = "All")]
        public string ConnectionName { get; set; }
    }
}
