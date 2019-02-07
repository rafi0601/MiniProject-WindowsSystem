//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public struct Address
    {
        public string Street { get; set; }
        public uint HouseNumber { get; set; }
        public string City { get; set; }


        public Address(string street, uint houseNumber, string city) : this()
        {
            Street = street;
            HouseNumber = houseNumber;
            City = city;
        }


        public override string ToString()
        {
            return $"{City}, {Street}, {HouseNumber}.";
        }
    }
}