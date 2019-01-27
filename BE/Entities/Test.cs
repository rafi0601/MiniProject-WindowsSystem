using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BE
{
    [Serializable]
    [DebuggerDisplay("Code: {Code}; tester's ID: {IdTester}, trainee's ID: {IdTrainee}")]
    public sealed class Test : IKey
    {
        string IKey.Key => Code; //TODO public

        //[Serializable]
        //public class Criteria
        //{
        //    private readonly bool?[] grades = new bool?[NumberOfCriteria];
        //
        //    private static readonly string[] namesOfCriterial = (from criterionName in Enum.GetNames(typeof(Criterion))
        //                                                         orderby criterionName
        //                                                         select criterionName.SplitByUpperAndToLower()).ToArray();
        //    public static readonly uint NumberOfCriteria = (uint)namesOfCriterial.Length;
        //    public static string[] NamesOfCriterial() => namesOfCriterial; // NOTE: is by reference or value??? (value is bad)
        //
        //    // TODO: private or public?
        //    Criteria() { }
        //    Criteria(Criteria criteria) { }
        //
        //    public bool? this[Criterion e]
        //    {
        //        get => grades[(uint)e];
        //        set => grades[(uint)e] = value;
        //    }
        //
        //    public uint PassGrades()
        //    {
        //        return (uint)grades.Count(grade => grade == true);
        //    }
        //
        //
        //}

        ///       
        ///   //אין קונסטרקטור כי כמו שיוצרים את התיבות סימון בפוראיצ על כל קריטריון כך עושים סטטר על כל קריטריון ממה שיש בתיבות
        ///   //להגדיר קבוע שאומר כמה תיבות צריך למלא במינימום (לא וי ולא איקס אלא ריק) ע
        ///   // לעשות איוונט של מה קורה שסיימו להזין ציונים ובתוכו פונקציה שעושה יילד רטורן של זוג סדור 
        ///
        ///  public class Criteria
        ///  {
        ///       public class _Criteria
        ///       {
        /// 
        ///           public struct Criterion
        ///           {
        ///               public readonly string Names;
        ///               private Criterion([CallerMemberName] string name = null) { Names = name.SplitAndLower(); }
        ///           }
        /// 
        ///           private static Criterion KeepDistance = new Criterion();
        ///           private static Criterion BackParking = new Criterion();
        ///           private static Criterion UsingViewMirrors = new Criterion();
        ///           private static Criterion Signaling = new Criterion();
        ///           private static Criterion IntegrationIntoMovement = new Criterion();
        ///           private static Criterion ObeyParkSigns = new Criterion();
        /// 
        ///           private static readonly PropertyInfo[] criteria = new Criteria().GetType().GetProperties();
        ///           public static string[] NamesOfCriterial() => (from criterionProperty in criteria
        ///                                                         where criterionProperty.PropertyType.get
        ///                                                         select (Criterion)criterionProperty.GetValue(criteria).  .ToArray();
        ///       }
        /// 
        ///       private readonly Dictionary<string, bool?> criteria = new Dictionary<string, bool?>(from criterion in criteria
        ///                                                                                           select criterion.Name.SplitAndLower()).ToArray())
        /// 
        ///  //     private readonly Dictionary<string, bool?> criteria = new Dictionary<string, bool?>(){
        ///  //     { "Keep distance", null }, {"Back parking", null }, {"Using view mirrors", null },
        ///  //     {"Signaling", null }, {"Integration into movement", null }, {"Obey park signs", null } };
        /// 
        ///       public static readonly uint NumberOfCriteria = (uint)new Criteria().criteria.Count;
        /// 
        ///       //   public bool? this[string name]
        ///       //   {
        ///       //       get => criteria[name];
        ///       //       set => criteria[name] = value != null ? value : throw new /*ArgumentOutOfRange*/Exception();
        ///       //   }
        /// 
        ///       public bool? this[_Criteria.Criterion criterion]
        ///       {
        ///           get => criteria[criterion.Name];
        ///           set => criteria[criterion.Name] = value != null ? value : throw new /*ArgumentOutOfRange*/Exception();
        ///       }
        /// 
        ///       public Criteria()
        ///       {
        ///           foreach (var v in _Criteria.Criterion criterion)
        ///           {
        ///               criteria[]= GetGrade()
        ///           }
        ///       }
        ///       public Criteria(Criteria criteria) { }
        /// 
        /// 
        ///   }
        ///   
        /// 
        ///   public bool KeepDistance { get; set; }
        ///   public bool BackParking { get; set; }
        ///   public bool UsingViewMirrors { get; set; }
        ///   public bool Signaling { get; set; }
        ///   public bool IntegrationIntoMovement { get; set; }
        ///   public bool ObeyParkSigns { get; set; }
        /// 
        ///   */


        public class Criteria
        {
            public bool? KeepDistance { get; set; }
            public bool? BackParking { get; set; }
            public bool? UsingViewMirrors { get; set; }
            public bool? Signaling { get; set; }
            public bool? IntegrationIntoMovement { get; set; }
            public bool? ObeyParkSigns { get; set; }

            public static readonly uint NumberOfCriteria = (uint)new Criteria().GetType().GetProperties().Length;

            public uint PassGrades()
            {
                return (uint)GetType().GetProperties().Count(property => (bool?)property.GetValue(this) ?? false);
            }
        }

        public struct TestDate
        {
            DateTime dateAndTime;

            //public static TestDate operator =(TestDate testDate, DateTime dateTime)
            //{
            //    testDate.dateAndTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            //    return testDate;
            //}

            public static implicit operator DateTime(TestDate testDate)
            {
                return testDate.dateAndTime;
            }
        }


        public string Code { get; set; }
        public string IDTester { get; }
        public string IDTrainee { get; }
        public DateTime Date { get; set; }
        //public DateTime Length { get; set; } //CHECK: what is this
        public Address DepartureAddress { get; set; }
        public Vehicle Vehicle { get; }

        public Criteria CriteriasGrades { get; set; }//=new Criteria()

        public bool? IsPass { get; set; }
        public string TesterNotes { get; set; }

        public bool IsDone() => DateTime.Now > Date + Configuration.LENGTH_OF_TEST; // IMPROVEMENT is better to change after updating 

        public Test(string idTester, string idTrainee, DateTime date,
            /*DateTime length,*/ Address departureAddress, Vehicle vehicle)
        {
            IDTester = idTester;
            IDTrainee = idTrainee;
            Date = date;
            //Length = length;
            DepartureAddress = departureAddress;
            Vehicle = vehicle;
        }

        private Test(string code, string idTester, string idTrainee, DateTime date,
            /*DateTime length,*/ Address departureAddress, Vehicle vehicle,
            Criteria criteriasGrades, bool? isPass, string testerNotes)
            : this(idTester, idTrainee, date,/*length,*/departureAddress, vehicle)
        {
            Code = code;
            CriteriasGrades = criteriasGrades;
            IsPass = isPass;
            TesterNotes = testerNotes;
        }

        public Test(Test test)
            : this(test.Code, test.IDTester, test.IDTrainee, test.Date,
                  /*test.Length,*/ test.DepartureAddress, test.Vehicle,
                  test.CriteriasGrades, test.IsPass, test.TesterNotes)
        { }

        public override string ToString()
        {
            return "Test " + Code;
            //return this.ToStringProperty();
        }
    }
}