//Bs"d

#define CalcLenthOfTest

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Xml.Serialization;
using static BE.Configuration;

namespace BE
{
    [Serializable]
    [DebuggerDisplay("Code: {Code}; tester's ID: {IdTester}, trainee's ID: {IdTrainee}")]
    public partial class Test : IKey
    {
        string IKey.Key => Code;

        public string Code { get; set; }
        public string TesterID { get; set; }
        public string TraineeID { get; set; }
        public DateTime Date { get; set; }
        [Obsolete(message: "For future propose", error: false)]
        public DateTime Length { get; set; } //CHECK: what is this
        public Address DepartureAddress { get; set; }
        public Vehicle Vehicle { get; set; }
        [XmlIgnore]
        public Criteria CriteriasGrades { get; set; }

        public bool? IsPass { get; set; }
        public string TesterNotes { get; set; }


        public bool IsDone() => DateTime.Now > Date + LENGTH_OF_TEST;


        public Test() { }

        public Test(string idTester, string idTrainee, DateTime date,
             Address departureAddress, Vehicle vehicle)
        {
            TesterID = idTester;
            TraineeID = idTrainee;
            Date = date;
            DepartureAddress = departureAddress;
            Vehicle = vehicle;
        }

        private Test(string code, string idTester, string idTrainee, DateTime date,
            Address departureAddress, Vehicle vehicle,
            Criteria criteriasGrades, bool? isPass, string testerNotes)
            : this(idTester, idTrainee, date, departureAddress, vehicle)
        {
            Code = code;
            CriteriasGrades = criteriasGrades;
            IsPass = isPass;
            TesterNotes = testerNotes;
        }

        public Test(Test test)
            : this(test.Code, test.TesterID, test.TraineeID, test.Date,
                  test.DepartureAddress, test.Vehicle,
                  test.CriteriasGrades, test.IsPass, test.TesterNotes)
        { }


        public override string ToString()
        {
            return "Test " + Code;
            //return this.ToStringProperty();
        }

        
        public string criteriasGrades
        {
            get
            {
                if (CriteriasGrades == null)
                    return "";

                StringBuilder result = new StringBuilder();
                foreach (PropertyInfo criteriaInfo in typeof(Criteria).GetProperties())
                    result.Append((criteriaInfo.GetValue(CriteriasGrades)?.ToString() ?? "null") + ",");
                return result.ToString();
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    CriteriasGrades = new Criteria();

                    int index = 0;
                    foreach (PropertyInfo criteriaInfo in typeof(Criteria).GetProperties())
                        criteriaInfo.SetValue(CriteriasGrades, values[index++] != "null" ? (bool?)bool.Parse(values[index - 1]) : null);
                }
                else
                    CriteriasGrades = new Criteria();
            }
        }
    }
}