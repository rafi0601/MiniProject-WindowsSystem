//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static BE.Configuration;

namespace BE
{
    [Serializable]
    public sealed class Tester : Person
    {
        public uint YearsOfExperience { get; set; }
        public uint MaxOfTestsPerWeek { get; set; }
        public Vehicle VehicleTypesExpertise { get; set; }
        [XmlIgnore]
        public Schedule WorkingHours { get; set; } = new Schedule();
        public uint MaxDistanceFromAddress { get; set; }
        public List<DateTime> UnavailableDates { get; set; } = new List<DateTime>(); //IMPROVEMENT sorted list
        //public SortedList<DateTime, DateTime> UnavailableDates = new SortedList<DateTime, DateTime>();// IMPROVMENT will take less time to search


        public Tester()
        { }

        public Tester(string id, PersonName name,
            DateTime birthdate, Gender gender, string mobileNumber,
            Address address, string password, uint yearsOfExperience,
            uint maxOfTestsPerWeek, Vehicle vehicleTypeExpertise,
            Schedule workingHours, uint maxDistanceFromAddress)
            : base(id, name, birthdate, gender, mobileNumber, address, password)
        {
            YearsOfExperience = yearsOfExperience;
            MaxOfTestsPerWeek = maxOfTestsPerWeek;
            VehicleTypesExpertise = vehicleTypeExpertise;
            WorkingHours = workingHours;
            MaxDistanceFromAddress = maxDistanceFromAddress;
        }

        public Tester(Tester tester)
             : this(tester.ID, tester.Name, tester.Birthdate, tester.Gender, tester.MobileNumber,
                   tester.Address, tester.Password, tester.YearsOfExperience, tester.MaxOfTestsPerWeek,
                   tester.VehicleTypesExpertise, tester.WorkingHours, tester.MaxDistanceFromAddress)
        { }


        public override string ToString()
        {
            return base.ToString() + ", tester";
            //return this.ToStringProperty();
        }


        // For XmlSerializer: they both work, we prefer the first one because it is more secure (harder to forge)
        public string workingHours
        {
            get
            {
                if (WorkingHours == null)
                    return null;

                StringBuilder result = new StringBuilder();
                for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
                    for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                        result.Append(WorkingHours[i, (int)(j + BEGINNING_OF_A_WORKING_DAY)] + ",");

                return result.ToString();
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] values = value.Split(',');

                    WorkingHours = new Schedule();

                    int index = 0;
                    for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
                        for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                            WorkingHours[i, (int)(j + BEGINNING_OF_A_WORKING_DAY)] = bool.Parse(values[index++]);
                }
            }
        }
        [XmlIgnore]
        public bool[][] workinghours 
        {
            get
            {
                if (WorkingHours == null)
                    return null;

                bool[][] result = new bool[WORKING_DAYS_A_WEEK][];
                for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
                {
                    result[i] = new bool[WORKING_HOURS_A_DAY];
                    for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                        result[i][j] = WorkingHours[i, (int)(j + BEGINNING_OF_A_WORKING_DAY)];
                }

                return result;
            }

            set
            {
                if (value != null && value.GetLength(0) == WORKING_DAYS_A_WEEK) // TODO send exception
                {
                    WorkingHours = new Schedule();

                    for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
                    {
                        if (value[1].GetLength(0) == WORKING_HOURS_A_DAY)
                            for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                                WorkingHours[i, (int)(j + BEGINNING_OF_A_WORKING_DAY)] = value[i][j];
                    }
                }
            }
        }
    }
}