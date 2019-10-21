using Extensions;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueView.Format
{
    public class TableFormat : MessageFormat
    {
        /// <summary>
        /// The widths for each of the columns in the table.
        /// SequenceNumber is a 64 bit int and needs 19 chars to fit the widest value
        /// </summary>
        private static readonly Dictionary<string, int> FieldWidths = new Dictionary<string, int>
        {
            { MessageId, 20 },
            { SeqNum, 20 },
            { Body, 100 },
            { UserProperties, 100 }
        };

        private static readonly Dictionary<string, Func<Message, string>> FieldMap = new Dictionary<string, Func<Message, string>>
        {
            { MessageId, message => message.MessageId.Truncate(FieldWidths[MessageId] - 1) },
            { SeqNum, message => message.SystemProperties.SequenceNumber.ToString().Truncate(FieldWidths[SeqNum] - 1) },
            { Body, message => GetBody(message).Truncate(FieldWidths[Body] - 1) },
            { UserProperties, message => JsonConvert.SerializeObject(message.UserProperties).Truncate(FieldWidths[UserProperties] - 1) }
        };

        private readonly string _columnFormat;

        public TableFormat(List<string> fields) : base(fields, FieldMap)
        {
            // Build the column format string for use in `string.Format`
            // If all columns are included the format string will look like {0,-20}{1,-20}{2,-100}{3,-100}
            StringBuilder columnFormatBuilder = new StringBuilder();

            for (int i = 0; i < Fields.Count; i++)
            {
                // Put the i'th field in the i'th position as seen in the first concatenation
                // Get the width of the field as specified in the FieldWidths map in the second concatenation
                // The negative sign means 'left align' to `string.Format`
                columnFormatBuilder.Append("{" + i + ",-" + FieldWidths[Fields[i]] + "}");
            }

            _columnFormat = columnFormatBuilder.ToString();
        }

        protected override string RowFormatter(List<string> list)
        {
            /*
             * The things to be formatted must be cast to an object[].
             * This is because `string.Format` only accepts varargs in the form object[].
             *
             * If the things to be formatted are passed as `List<string>` then `string.Format`
             * will match the whole list to the formatted object {0} and then fail when trying
             * to format object {1}.
             *
             * The exception will report there is no {1} because the whole list was consumed
             * as object {0}
             *
             */
            object[] args = new object[list.Count];

            for (int i=0; i<list.Count; i++)
            {
                args[i] = list[i];
            }

            return string.Format(_columnFormat, args);
        }
    }
}
