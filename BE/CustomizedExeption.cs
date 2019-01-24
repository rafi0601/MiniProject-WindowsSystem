using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CustomizedExeption :Exception
    {
        public bool DisplayToUser { get; }


        public CustomizedExeption(bool displayToUser, string message, Exception innerException)
            : base(message, innerException)
        {
            DisplayToUser = displayToUser;
        }

        //protected CustomizedExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }
}
