//BS"D

using System;
using System.Runtime.CompilerServices;

namespace BE
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UserDisplayAttribute : Attribute
    {
        public UserDisplayAttribute(/*[CallerMemberName]*/ string displayName = null)
        {
            DisplayName = displayName.SplitByUpperAndToLower();
        }

        public string DisplayName { get; }
    }
}