using Microsoft.Azure.ServiceBus;

namespace QueueView.Format
{
    public interface IMessageFormat
    {
        /// <summary>
        /// Formats headers appropriately per the implementation.
        /// </summary>
        /// <returns>A single line string of formatted headers.</returns>
        string FormatHeaders();
        
        /// <summary>
        /// Formats a message appropriately per the implementation.
        /// </summary>
        /// <returns>A single line string of a formatted message.</returns>
        string Format(Message message);
    }
}
