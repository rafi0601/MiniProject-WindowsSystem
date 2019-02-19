using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactorySingleton
    {
        protected static IBL instance = null;
        private static readonly object padlock = new object();

        protected FactorySingleton() { }

        public static IBL Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new Bl_imp();

                    return instance;
                }
            }
        }
    }


}
