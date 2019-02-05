//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        public Vehicle VehicleTypeExpertise { get; set; } // TODO rename to VTypesE
        [XmlIgnore]
        public bool[,] WorkingHours { get; set; } = new bool[Configuration.WORKING_DAYS_A_WEEK, Configuration.WORKING_HOURS_A_DAY];
        public string workingHours
        {
            get
            {
                if (WorkingHours == null)
                    return null;

                string result = "";
                if (WorkingHours != null)
                {
                    int sizeA = WorkingHours.GetLength(0);
                    int sizeB = WorkingHours.GetLength(1);
                    result += "" + sizeA + "," + sizeB;

                    for (int i = 0; i < sizeA; i++)
                        for (int j = 0; j < sizeB; j++)
                            result += "," + WorkingHours[i, j];
                }

                return result;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    int sizeA = int.Parse(values[0]);
                    int sizeB = int.Parse(values[1]);
                    WorkingHours = new bool[sizeA, sizeB];

                    int index = 2;
                    for (int i = 0; i < sizeA; i++)
                        for (int j = 0; j < sizeB; j++)
                            WorkingHours[i, j] = bool.Parse(values[index++]);
                }
            }
        }
        public uint MaxDistanceFromAddress { get; set; }

        //public List<Test> MyTests = new List<Test>();
        [XmlIgnore]
        public List<DateTime> MyTests = new List<DateTime>();
        public string myTests
        {
            get
            {
                if (MyTests == null)
                    return null;

                string result = "";
                foreach (var testDateTime in myTests)
                    result += "," + testDateTime.ToString();

                return result;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    MyTests = new List<DateTime>();

                    for (int i = 0; i < values.Length; i++)
                        MyTests.Add(DateTime.Parse(values[i]));
                }
            }
        }
        //public SortedList<DateTime, DateTime> MyTests = new SortedList<DateTime, DateTime>();// IMPROVMENT will take less time to search

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