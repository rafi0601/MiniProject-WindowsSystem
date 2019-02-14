//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public enum Gender
    {
        male = 1,
        female

        /// It is like that (male=1 and female=2) because:
        /// male, female
        ///     is forbidden because then male=0 (and female=1)
        /// and 
        /// female, male 
        ///     is forbidden because the female before the male
        /// :)
    }

    [Flags]
    public enum Vehicle : uint
    {
        [UserDisplay("Private car")]
        privateCar = 0b1, //1<<0,

        [UserDisplay("Two wheeled")]
        twoWheeledVehicle = 0b10, //1<<1

        [UserDisplay("Medium truck")]
        mediumTruck = 0b100,//1<<2,

        [UserDisplay("Heavy truck")]
        heavyTruck = 0b1_000, //1<<3,

        [UserDisplay("Public vehicle")]
        publicVehicle = 0b10_000, //1<<4,

        [UserDisplay("Tractor")]
        tractor = 0b100_000 //1<<5,
    }

    public enum Gearbox
    {
        automatic = 1,
        manual

        /// In this way, the user must select a vehicle type
    }

    //public enum Hours:uint
    //{
    //    nine=9,
    //    ten=10,
    //    eleven=11,
    //    twelve=12,
    //}


    //public enum Grade : short
    //{
    //    pass, failed
    //}
}