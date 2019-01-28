﻿//BS"D

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
        // TODO remove the properties
        public string Street { get; set; }
        public uint HouseNumber { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return $"{City}, {Street}, {HouseNumber}.";
        }
    }
}