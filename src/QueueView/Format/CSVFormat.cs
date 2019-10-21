using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QueueView.Format
{
    public class CsvFormat : MessageFormat
    {
        /// <summary>
        /// Map the fields of a message to their full string representation.
        /// </summary>
        private static readonly Dictionary<string, Func<Message, string>> FieldMap = new Dictionary<string, Func<Message, string>>
        {
            { MessageId, message => message.MessageId },
            { SeqNum, message => message.SystemProperties.SequenceNumber.ToString() },
            { Body, GetBody },
            { UserProperties, message => JsonConvert.SerializeObject(message.UserProperties) }
        };

        public CsvFormat(List<string> fields) : base(fields, FieldMap)
        { }

        /// <summary>
        /// Join the fields together in comma-separated style.
        /// </summary>
        /// <param name="list">The values to be formatted.</param>
        /// <returns>The <see cref="list"/> joined by commas.</returns>
        protected override string RowFormatter(List<string> list)
        {
            return string.Join(", ", list);
        }
    }
}
