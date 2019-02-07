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
    public class CustomException : Exception
    {
        public bool DisplayToUser { get; }
        //public bool IsCritical { get; set; }

        public override string Message => InnerException.Message;


        public CustomException(bool displayToUser, Exception innerException)
            : base(message: null, innerException: innerException)
        {
            DisplayToUser = displayToUser;
            HelpLink = ""; //UNDONE link to our git hub?
        }

        // עושים את מה שכתוב למעלה כשכותבים אוורייד ורואים מה חוזר
        //protected CustomizedExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
        

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
