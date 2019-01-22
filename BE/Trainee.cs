//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Trainee : Person
    {
        public Vehicle Vehicle { get; set; }
        public Gearbox Gearbox { get; set; }
        public string DrivingSchool { get; set; }
        public Name TeacherName { get; set; } = new Name();
        public uint NumberOfDoneLessons { get; set; }
        public DateTime TheLastTest { get; set; }

        public Trainee() { }

        public Trainee(string id, Name name, DateTime birthdate, Gender gender,
            string phoneNumber, Address address, Vehicle vehicle, Gearbox gearbox,
            string drivingSchool, Name teacherName, uint numberOfDoneLessons) //UNDONE DT theLastTest
            : base(id, name, birthdate, gender, phoneNumber, address)
        {
            Vehicle = vehicle;
            Gearbox = gearbox;
            DrivingSchool = drivingSchool;
            TeacherName = teacherName;
            NumberOfDoneLessons = numberOfDoneLessons;
        }

        public Trainee(Trainee trainee)
            : this(trainee.ID, trainee.Name, trainee.Birthdate, trainee.Gender,
                  trainee.PhoneNumber, trainee.Address, trainee.Vehicle, trainee.Gearbox,
                  trainee.DrivingSchool, trainee.TeacherName, trainee.NumberOfDoneLessons)
        { }


        public override string ToString()
        {
            return base.ToString() + ", trainee";
        }
    }
}