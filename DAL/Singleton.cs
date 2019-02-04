using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public /*static*/ abstract class Singleton
    {
        protected static IDal instance = null;

        //protected Singleton() { }

        public static IDal Instance
        {
            get
            {
                if (instance == null)
                    //instance = new Dal_ListsImp();
                    instance = new Dal_XmlImp();

                return instance;
            }
        }
    }

    //  public class Singleton
    //  {
    //      protected Singleton() { }
    //      public static IDal GetDal { get; } = BE.Singleton<IDal, Dal_ListImp>.Instance;
    //  }
}
