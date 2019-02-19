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
    public class CasingException : Exception
    {
        public bool DisplayToUser { get; }
        //public bool IsCritical { get; set; }

        public override string Message => InnerException.Message;


        public CasingException(bool displayToUser, Exception innerException)
            : base(message: null, innerException: innerException)
        {
            DisplayToUser = displayToUser;
            HelpLink = ""; //UNDONE link to our git hub?
        }
        
        
        // TODO more ovverides


        public override string ToString()
        {
            return InnerException.ToString();
        }
    }
}
