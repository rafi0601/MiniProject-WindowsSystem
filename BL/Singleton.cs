using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Singleton
    {
        protected static IBL instance = null;

        protected Singleton() { }

        public static IBL Instance
        {
            get
            {
                if (instance == null)
                    instance = new Bl_imp();
                return instance;
            }
        }
    }


}
