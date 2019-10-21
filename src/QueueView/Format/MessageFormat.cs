using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.ServiceBus.InteropExtensions;

namespace QueueView.Format
{
    public abstract class MessageFormat : IMessageFormat
    {
        public const string MessageId = "MessageId";
        public const string SeqNum = "SeqNum";
        public const string Body = "Body";
        public const string UserProperties = "User Properties";

        public static readonly List<string> AllFields = new List<string>
        {
            MessageId,
            SeqNum,
            Body,
            UserProperties
        };
        
        protected List<string> Fields;

        protected Dictionary<string, Func<Message, string>> FieldMapper;

        protected MessageFormat(List<string> fields, Dictionary<string, Func<Message, string>> fieldMapper)
        {
            Fields = fields.Any() ? fields : AllFields;
            FieldMapper = fieldMapper;
        }

        public string FormatHeaders()
        {
            return RowFormatter(Fields);
        }

        public string Format(Message message)
        {
            List<string> formattedFields = new List<string>();

            foreach (string field in Fields)
            {
                formattedFields.Add(FieldMapper[field](message));
            }

            return RowFormatter(formattedFields);
        }

        public static string GetBody(Message message)
        {
            try
            {
                string bodyAsString = message.GetBody<string>();
                return bodyAsString;
            }
            catch (Exception)
            {
                string bodyAsBytes = Encoding.UTF8.GetString(message.Body);
                return bodyAsBytes;
            }
        }

        protected abstract string RowFormatter(List<string> list);
    }
}
