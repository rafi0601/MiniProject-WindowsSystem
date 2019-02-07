//BS"D

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


        public ExistingInTheDatabaseException(bool isExist)
        {
            IsExist = isExist;
        }

        public ExistingInTheDatabaseException(bool isExist, string message)
            : base(message)
        {
            IsExist = isExist;
        }

        public ExistingInTheDatabaseException(bool isExist, string message, Exception innerException)
            : base(message, innerException)
        {
            IsExist = isExist;
        }

        protected ExistingInTheDatabaseException(bool isExist, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            IsExist = isExist;
        }
    }
}
