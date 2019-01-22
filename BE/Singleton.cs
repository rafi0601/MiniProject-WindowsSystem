using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Singleton<Interface, Implementaion> where Implementaion : Interface, new() // CHECK where Interface:interface
    {
        protected static Interface instance = default(Interface);

        protected Singleton() { }

        public static Interface Instance
        {
            get
            {
                if (instance == null)
                    instance = new Implementaion();
                return instance;
            }
        }
    }
}
