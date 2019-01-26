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

        public CustomException(bool displayToUser, /*string message,*/ Exception innerException)
            : base(/*message*/null, innerException)
        {
            DisplayToUser = displayToUser;
            HelpLink = ""; //UNDONE link to our git hub?
        }
        // TODO לעשות אווריד שדורס את ההודעה ומחזיר את ההודעה של החריגה הפנימית
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
