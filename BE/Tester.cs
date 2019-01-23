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
    public class Tester : Person
    {
        public uint YearsOfExperience { get; set; }
        public uint MaxOfTestsPerWeek { get; set; }
        public Vehicle VehicleTypeExpertise { get; set; }
        public bool[,] WorkingHours { get; set; } = new bool[Configuration.WORKING_DAYS_A_WEEK, Configuration.WORKING_HOURS_A_DAY];
        public uint MaxDistanceFromAddress { get; set; }
        public List<Test> MyTests = new List<Test>();
        //SortedSet<Test> tests = new SortedSet<Test>(); IMPROVMENT will take less time to search

        public Tester(string id, Name name,
            DateTime birthdate, Gender gender, string phoneNumber,
            Address address, uint yearsOfExperience,
            uint maxOfTestsPerWeek, Vehicle vehicleTypeExpertise,
            bool[,] workingHours, uint maxDistanceFromAddress)
            : base(id, name, birthdate, gender, phoneNumber, address)
        {
            YearsOfExperience = yearsOfExperience;
            MaxOfTestsPerWeek = maxOfTestsPerWeek;
            VehicleTypeExpertise = vehicleTypeExpertise;
            WorkingHours = workingHours;
            MaxDistanceFromAddress = maxDistanceFromAddress;
        }

        public Tester(Tester tester)
             : this(tester.ID, tester.Name, tester.Birthdate, tester.Gender, tester.PhoneNumber,
                   tester.Address, tester.YearsOfExperience, tester.MaxOfTestsPerWeek,
                   tester.VehicleTypeExpertise, tester.WorkingHours, tester.MaxDistanceFromAddress)
        { }

        public override string ToString()
        {
            return base.ToString() + ", tester";
        }
    }
}