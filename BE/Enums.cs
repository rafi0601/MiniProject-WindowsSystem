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

        /// It is like that because:
        /// male, female
        ///     is forbidden because then male=0 (and female=1)
        /// and 
        /// female, male 
        ///     is forbidden because the female before the male
        /// And when it's like that male=1 and female=2
    }

    [Flags]
    public enum Vehicle
    {
        privateCar = 0b1,
        twoWheeledVehicle = 0b10,
        mediumTruck = 0b100,
        heavyTruck = 0b1_000,
        publicVehicle = 0b10_000,
        tractor = 0b100_000
    }

    public enum Gearbox
    {
        automatic,
        manual
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