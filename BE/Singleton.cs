//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BE
//{
//    public class Singleton<Interface, Implementaion> where Implementaion : Interface, new() 
//    {
//        protected static Interface instance = default;


//        protected Singleton() { }

//        public static Interface Instance
//        {
//            get
//            {
//                if (instance == null)
//                    instance = new Implementaion();
//                return instance;
//            }
//        }
//    }
//}
