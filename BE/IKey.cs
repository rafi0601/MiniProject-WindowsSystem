using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IKey//<T> //TODO : IEqualityComparer<Person>
    {
        //T Key { get; }
        string Key { get; }
    }
}
