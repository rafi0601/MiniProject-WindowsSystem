using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class ExistingInTheDatabaseException : Exception
    {
        bool IsExist { get; }

        public ExistingInTheDatabaseException()
        {
        }

        public ExistingInTheDatabaseException(string message) : base(message)
        {
        }

        public ExistingInTheDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistingInTheDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
