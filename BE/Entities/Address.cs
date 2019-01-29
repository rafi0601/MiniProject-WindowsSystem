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
        private string _street;
        private uint _houseNumber;
        private string _city;

        // TODO remove the properties
        public string Street
        {
            get => _street;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Srteet mustn't be null or empty or consists only white spaces");

                _street = value;
            }
        }
        public uint HouseNumber
        {
            get => _houseNumber;
            set
            {
                if (value == 0)
                    throw new ArgumentOutOfRangeException("House number mustn't be zero");

                _houseNumber = value;
            }
        }
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("City mustn't be null or empty or consists only white spaces");

                _city = value;
            }
        }
        //public string Street { get; set; }
        //public uint HouseNumber { get; set; }
        //public string City { get; set; }

        public override string ToString()
        {
            return $"{City}, {Street}, {HouseNumber}.";
        }
    }
}