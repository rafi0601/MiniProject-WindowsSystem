//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// 
    /// </summary>
    public enum Gender
    {
        male = 1,
        female
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
}