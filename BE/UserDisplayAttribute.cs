//BS"D

using System;
using System.Runtime.CompilerServices;

namespace BE
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class UserDisplayAttribute : Attribute
    {
        public string DisplayName { get; }


        public UserDisplayAttribute(/*[CallerMemberName]*/ string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}