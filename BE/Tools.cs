﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

    }
}
