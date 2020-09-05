using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    public class MissingException : Exception, ISerializable
    {
        public MissingException() : base() { }
        public MissingException(string message) : base(message) { }
        public MissingException(string message, Exception inner) : base(message, inner) { }
        protected MissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string ToString()
        {
            return "MissingException: DAL missing " + Message + "\n";
        }
    }
}
