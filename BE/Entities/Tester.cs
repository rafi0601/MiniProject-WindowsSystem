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
        [XmlIgnore]
        public List<DateTime> UnavailableDates { get; set; } = new List<DateTime>(); //IMPROVEMENT sorted list
        //public SortedList<DateTime, DateTime> UnavailableDates = new SortedList<DateTime, DateTime>();// IMPROVMENT will take less time to search


        public Tester()
        { }

        public Tester(string id, PersonName name,
            DateTime birthdate, Gender gender, string phoneNumber,
            Address address, string password, uint yearsOfExperience,
            uint maxOfTestsPerWeek, Vehicle vehicleTypeExpertise,
            Schedule workingHours, uint maxDistanceFromAddress)
            : base(id, name, birthdate, gender, phoneNumber, address, password)
        {
            YearsOfExperience = yearsOfExperience;
            MaxOfTestsPerWeek = maxOfTestsPerWeek;
            VehicleTypesExpertise = vehicleTypeExpertise;
            WorkingHours = workingHours;
            MaxDistanceFromAddress = maxDistanceFromAddress;
        }

        public Tester(Tester tester)
             : this(tester.ID, tester.Name, tester.Birthdate, tester.Gender, tester.PhoneNumber,
                   tester.Address, tester.Password, tester.YearsOfExperience, tester.MaxOfTestsPerWeek,
                   tester.VehicleTypesExpertise, tester.WorkingHours, tester.MaxDistanceFromAddress)
        { }


        public override string ToString()
        {
            return base.ToString() + ", tester";
            //return this.ToStringProperty();
        }


        public string unavailableDates
        {
            get
            {
                if (UnavailableDates == null)
                    return "";

                string result = "";
                foreach (var testDateTime in UnavailableDates)
                    result += testDateTime.ToString() + ",";

                return result;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    UnavailableDates = new List<DateTime>();

                    for (int i = 0; i < values.Length - 1; i++)
                        UnavailableDates.Add(DateTime.Parse(values[i]));
                }
                else
                    UnavailableDates = new List<DateTime>();
            }
        }
        public string workingHours
        {
            get
            {
                if (WorkingHours == null)
                    return null;

                StringBuilder result = new StringBuilder();
                if (WorkingHours != null)
                {
                    for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
                        for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                            result.Append(WorkingHours[i, (int)(j+BEGINNING_OF_A_WORKING_DAY)] + ",");
                }

                return result.ToString();
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    WorkingHours = new Schedule();

                    int index = 0;
                    for (ushort i = 0; i < WORKING_DAYS_A_WEEK; i++)
                        for (ushort j = 0; j < WORKING_HOURS_A_DAY; j++)
                            WorkingHours[i, (int)(j + BEGINNING_OF_A_WORKING_DAY)] = bool.Parse(values[index++]);
                }
            }
        }
    }
}