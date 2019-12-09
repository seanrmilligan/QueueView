using CommandLine;

namespace QueueView.Arguments
{
    [Verb("queues", HelpText = "Operations on queues. See 'queues --help' for more.")]
    public class QueueOptions
    {
        [Option('c', "connection", HelpText = "The name of the connection to use if not the default.")]
        public string ConnectionName { get; set; }
    }
}
