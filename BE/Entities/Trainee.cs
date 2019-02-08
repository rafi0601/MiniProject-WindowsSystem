//Bs"d

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
        public PersonName TeacherName { get; set; }
        public uint NumberOfDoneLessons { get; set; }
        public DateTime TheLastTest { get; set; }


        public Trainee() { }

        public Trainee(string id, PersonName name, DateTime birthdate, Gender gender,
            string phoneNumber, Address address, string password, Vehicle vehicleTypeTraining, Gearbox gearboxTypeTraining,
            string drivingSchool, PersonName teacherName, uint numberOfDoneLessons, DateTime theLastTest = default) //UNDONE DateTime theLastTest
            : base(id, name, birthdate, gender, phoneNumber, address, password)
        {
            VehicleTypeTraining = vehicleTypeTraining;
            GearboxTypeTraining = gearboxTypeTraining;
            DrivingSchool = drivingSchool;
            TeacherName = teacherName;
            NumberOfDoneLessons = numberOfDoneLessons;
            TheLastTest = theLastTest;
        }

        public Trainee(Trainee trainee)
            : this(trainee.ID, trainee.Name, trainee.Birthdate, trainee.Gender,
                  trainee.PhoneNumber, trainee.Address, trainee.Password, trainee.VehicleTypeTraining, trainee.GearboxTypeTraining,
                  trainee.DrivingSchool, trainee.TeacherName, trainee.NumberOfDoneLessons, trainee.TheLastTest)
        { }


        public override string ToString()
        {
            return base.ToString() + ", trainee";
            //return this.ToStringProperty();
        }
    }
}