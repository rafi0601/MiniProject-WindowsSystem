using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
            if(!typeof(T).IsSerializable)
                throw new CustomException(false, new ArgumentException("The type must be serializable.", "source")); // CHECK nameof(source)
            if (ReferenceEquals(source, null))
                return default(T);

            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream=new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
