using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Linq;

namespace BE
{
    public static class Tools
    {
        internal static string SplitByUpperAndToLower(this string str)
        {
            return new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace).Replace(str, " ").ToLower();
        }

        //  public static Array SplitByUpperAndLower(this Array array) 
        //  {
        //      var v= from str in array select str.SplitByUpperAndToLower();
        //      Array.ForEach(array, item => (item as string).SplitByUpperAndToLower());
        //  }

        internal static string ToStringProperty<T>(this T t)
        {
            StringBuilder str = new StringBuilder();
            foreach (PropertyInfo property in t.GetType().GetProperties(BindingFlags.Public))
                str.Append(property.Name + ": " + property.GetValue(t, null) + "\n"); //BUG if property is indexer
            return str.ToString(); // CHECK if is work
        }

        public static T Copy<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new CustomException(false, new ArgumentException("The type must be serializable.", "source")); // CHECK nameof(source)
            if (ReferenceEquals(source, null))
                return default(T);

            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            //return list is null ? true : (0 == list.Count); // count==0
            return list?.Any() ?? true;
        }

        public static Trainee FromXElement(XElement xElement)
        {
            Trainee tmp = new Trainee();

            XElement nameXElement = xElement.Element(nameof(tmp.Name));
            XElement addressXElement = xElement.Element(nameof(tmp.Address));
            XElement teacherNameXElement = xElement.Element(nameof(tmp.TeacherName));
            return new Trainee(
                xElement.Element(nameof(tmp.ID)).Value,
                new Name(nameXElement.Element(nameof(tmp.Name.LastName)).Value, nameXElement.Element(nameof(tmp.Name.FirstName)).Value),
                DateTime.Parse(xElement.Element(nameof(tmp.Birthdate)).Value),
                (Gender)Enum.Parse(typeof(Gender), xElement.Element(nameof(tmp.Gender)).Value),
                xElement.Element(nameof(tmp.PhoneNumber)).Value,
                new Address(addressXElement.Element(nameof(tmp.Address.Street)).Value, uint.Parse(addressXElement.Element(nameof(tmp.Address.HouseNumber)).Value), addressXElement.Element(nameof(tmp.Address.City)).Value),
                (Vehicle)Enum.Parse(typeof(Vehicle), xElement.Element(nameof(tmp.VehicleTypeTraining)).Value),
                (Gearbox)Enum.Parse(typeof(Gearbox), xElement.Element(nameof(tmp.GearboxTypeTraining)).Value),
                xElement.Element(nameof(tmp.DrivingSchool)).Value,
                new Name(teacherNameXElement.Element(nameof(tmp.TeacherName.LastName)).Value, teacherNameXElement.Element(nameof(tmp.TeacherName.FirstName)).Value),
                uint.Parse(xElement.Element(nameof(tmp.NumberOfDoneLessons)).Value));
        }

        public static T AddFlag<T>(this Enum type, T value) => (T)(object)((int)(object)type | (int)(object)value);

        public static T RemoveFlag<T>(this Enum type, T value) => (T)(object)((int)(object)type & ~(int)(object)value);

    }
}
