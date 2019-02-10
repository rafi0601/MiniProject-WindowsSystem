//BS"D

using System;

namespace BE
{
    public abstract partial class Person
    {
        [Serializable]
        public struct PersonName:IComparable
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }

            public PersonName(string lastName, string firstName) : this()
            {
                LastName = lastName;
                FirstName = firstName;
            }

            public override string ToString()
            {
                return LastName + " " + FirstName;
            }

            public int CompareTo(object obj)
            {
                return ToString().CompareTo(obj.ToString()); //TODO inputcheck if obl==NAME
            }
        }
    }
}