//BS"D

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
        #region strings

        internal static string SplitByUpperAndToLower(this string str)
        {
            return new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace).Replace(str, " ").ToLower();
        }

        internal static string ToStringProperty<T>(this T t)
        {
            StringBuilder str = new StringBuilder();
            foreach (PropertyInfo property in t.GetType().GetProperties(BindingFlags.Public))
                str.Append(property.Name + ": " + property.GetValue(t, null) + "\n"); //CHECK if work when property is indexer
            return str.ToString();
        }

        #endregion

        #region Flags

        public static T AddFlag<T>(this Enum type, T value)
            => (T)(object)((uint)(object)type | (uint)(object)value);

        public static T RemoveFlag<T>(this Enum type, T value)
            => (T)(object)((uint)(object)type & ~(uint)(object)value);

        public static bool IsFlag(this Enum type)
            => Convert.ToString((uint)(object)type, 2).Count(bit => bit == '1') == 1;

        //public static bool IsFlag<T>(this T type) where T : struct, Enum
        //    => Convert.ToString((uint)(object)type, 2).Count(bit => bit == '1') == 1;

        #endregion

        #region UserDisplayAttribute

        public static UserDisplayAttribute GetUserDisplayAttribute(Enum item)
        {
            if (item == null)
                throw new ArgumentNullException("No attributes for null");

            Type theAttributeType = typeof(UserDisplayAttribute);
            object[] arr = item.GetType().GetField(item.ToString()).GetCustomAttributes(theAttributeType, false);
            if (arr.Length == 1)
                return (UserDisplayAttribute)arr[0];
            return null;
        }

        public static Enum GetEnumAccordingToUserDisplay(Type enumType, string display)
        {
            if (enumType == null)
                throw new ArgumentNullException("No enum for null");
            if (!enumType.IsEnum)
                throw new ArgumentException("No enum for non enum");

            foreach (Enum field in Enum.GetValues(enumType))
            {
                UserDisplayAttribute attribute = GetUserDisplayAttribute(field);
                if (attribute?.DisplayName == display)
                    return field;
            }
            return null;
        }

        #endregion

        #region All

        public static T Copy<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException($"The type of {nameof(source)} must be serializable.");
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

        #endregion

        #region Various

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list?.Any() ?? true;
        }

        #endregion
    }
}
