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

        /// male, female
        ///     is forbidden because then male=0 (and female=1)
        /// and 
        /// female, male 
        ///     is forbidden because the female before the male
    }


    public enum Vehicle
    {
        privateCar,
        twoWheeledVehicle,
        mediumTruck,
        heavyTruck,
        publicVehicle,
        tractor
    }

    public enum Gearbox
    {
        automatic,
        manual
    }


    public enum Criterion : uint
    {
        KeepDistance,
        BackParking,
        UsingViewMirrors,
        Signaling,
        IntegrationIntoMovement,
        ObeyParkSigns,
    }

    //public enum Grade : short
    //{
    //    pass, failed
    //}
}