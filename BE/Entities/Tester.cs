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
    [Serializable]
    public sealed class Tester : Person
    {
        public uint YearsOfExperience { get; set; }
        public uint MaxOfTestsPerWeek { get; set; }
        public Vehicle VehicleTypeExpertise { get; set; }
        public bool[,] WorkingHours { get; set; } = new bool[Configuration.WORKING_DAYS_A_WEEK, Configuration.WORKING_HOURS_A_DAY];
        public uint MaxDistanceFromAddress { get; set; }
        public List<Test> MyTests = new List<Test>();
        //public SortedList<DateTime, Test> MyTests = new SortedList<DateTime, Test>();// IMPROVMENT will take less time to search

        [System.Runtime.CompilerServices.IndexerName("WoHo")]
        public bool this[DayOfWeek day, uint hour]
        {
            get
            {
                if (day > DayOfWeek.Thursday ||
                    hour < Configuration.BEGINNING_OF_A_WORKING_DAY ||
                    hour >= Configuration.BEGINNING_OF_A_WORKING_DAY + Configuration.WORKING_HOURS_A_DAY)
                    throw new Exception();

                return WorkingHours[(uint)day, (uint)hour];
            }
            private set => WorkingHours[(uint)day, (uint)hour] = value;
        }


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

        public Tester()
        {
        }

        public override string ToString()
        {
            return base.ToString() + ", tester";
            //return this.ToStringProperty();
        }
    }
}