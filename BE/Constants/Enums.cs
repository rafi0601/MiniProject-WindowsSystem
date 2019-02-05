﻿//BS"D

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
    }

    [Flags]
    public enum Vehicle
    {
        [UserDisplay(nameof(privateCar))]
        privateCar = 0b1,
        [UserDisplay()]
        twoWheeledVehicle = 0b10, //1<<1
        [UserDisplay]
        mediumTruck = 0b100,//1<<2
        [UserDisplay("Hi Reb Kidron")]
        heavyTruck = 0b1_000,
        publicVehicle = 0b10_000,
        tractor = 0b100_000
    }

    public enum Gearbox
    {
        automatic,
        manual
    }

    public enum WorkingHours:uint
    {
        nine=9,
        ten=10,
        eleven=11,
        twelve=12,
    }

    //public enum Criterion : uint
    //{
    //    KeepDistance,
    //    BackParking,
    //    UsingViewMirrors,
    //    Signaling,
    //    IntegrationIntoMovement,
    //    ObeyParkSigns,
    //}

    //public enum Grade : short
    //{
    //    pass, failed
    //}
}