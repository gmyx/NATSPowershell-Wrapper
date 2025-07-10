using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSWrapper
{
    public class NATSMessage
    {
        public readonly string Subject;
        public readonly string Message;

        internal NATSMessage(string _subject, string _message)
        {
            Subject = _subject;
            Message = _message;
        }
    }
}