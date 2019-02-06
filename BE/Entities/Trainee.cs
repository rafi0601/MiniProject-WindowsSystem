﻿//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public sealed class Trainee : Person
    {
        public Vehicle VehicleTypeTraining { get; set; }
        public Gearbox GearboxTypeTraining { get; set; }
        public string DrivingSchool { get; set; }
        public Name TeacherName { get; set; } = new Name();
        public uint NumberOfDoneLessons { get; set; }
        public DateTime TheLastTest { get; set; }

        public Trainee() { }

        public Trainee(string id, Name name, DateTime birthdate, Gender gender,
            string phoneNumber, Address address, string password, Vehicle vehicleTypeTraining, Gearbox gearboxTypeTraining,
            string drivingSchool, Name teacherName, uint numberOfDoneLessons) //UNDONE DT theLastTest
            : base(id, name, birthdate, gender, phoneNumber, address, password)
        {
            VehicleTypeTraining = vehicleTypeTraining;
            GearboxTypeTraining = gearboxTypeTraining;
            DrivingSchool = drivingSchool;
            TeacherName = teacherName;
            NumberOfDoneLessons = numberOfDoneLessons;
        }

        public Trainee(Trainee trainee)
            : this(trainee.ID, trainee.Name, trainee.Birthdate, trainee.Gender,
                  trainee.PhoneNumber, trainee.Address, trainee.Password, trainee.VehicleTypeTraining, trainee.GearboxTypeTraining,
                  trainee.DrivingSchool, trainee.TeacherName, trainee.NumberOfDoneLessons)
        { }


        public override string ToString()
        {
            return base.ToString() + ", trainee";
            //return this.ToStringProperty();
        }
    }
}