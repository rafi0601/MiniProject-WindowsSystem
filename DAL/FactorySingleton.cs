using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public /*static*/ abstract class FactorySingleton
    {
        protected static IDal instance = null;
        private static readonly object padlock = new object();

        protected FactorySingleton() { }

        public static IDal Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        //instance = new Dal_ListsImp();
                        instance = new Dal_XmlImp();

                    return instance;
                }
            }
        }
    }
}
