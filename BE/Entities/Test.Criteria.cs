//Bs"d

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BE
{
    public sealed partial class Test
    {
        [Serializable]
        public class Criteria : IEnumerable<PropertyInfo>
        {
            [UserDisplay("Keep Distance")]
            public bool? KeepDistance { get; set; }

            [UserDisplay("Back Parking")]
            public bool? BackParking { get; set; }

            [UserDisplay("UsingViewMirrors")]
            public bool? UsingViewMirrors { get; set; }

            [UserDisplay("Signaling")]
            public bool? Signaling { get; set; }

            [UserDisplay("Integration Into Movement")]
            public bool? IntegrationIntoMovement { get; set; }

            [UserDisplay("Obey Park Signs")]
            public bool? ObeyParkSigns { get; set; }


            public static readonly uint NumberOfCriteria = (uint)new Criteria().GetType().GetProperties().Length;

            public uint PassGrades()
            {
                return (uint)GetType().GetProperties().Count(property => (bool?)property.GetValue(this) == true);
            }


            public IEnumerator<PropertyInfo> GetEnumerator()
            {
                foreach (PropertyInfo criterion in GetType().GetProperties())
                {
                    yield return criterion;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }


            public Criteria() { }

            public Criteria(bool? keepDistance, bool? backParking, bool? usingViewMirrors, bool? signaling, bool? integrationIntoMovement, bool? obeyParkSigns)
            {
                KeepDistance = keepDistance;
                BackParking = backParking;
                UsingViewMirrors = usingViewMirrors;
                Signaling = signaling;
                IntegrationIntoMovement = integrationIntoMovement;
                ObeyParkSigns = obeyParkSigns;
            }
        }

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
    }
}