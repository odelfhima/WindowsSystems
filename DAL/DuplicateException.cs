using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    public class DuplicateException : Exception, ISerializable
    {
        public DuplicateException() : base() { }
        public DuplicateException(string message) : base(message) { }
        public DuplicateException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string ToString()
        {
            return "DuplicateException: DAL Duplicate " + Message + "\n";
        }
    }
}
