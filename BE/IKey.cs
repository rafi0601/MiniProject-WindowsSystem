//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IKey //IMPROVEMENT:ISerializable
                          //TODO : IEqualityComparer<Person>,
    {
        //T Key { get; }
        string Key { get; }
    }
}
