//BS"D

using System;

namespace BE
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UserDisplayAttribute : Attribute
    {
        public UserDisplayAttribute(string displayName)
        {
            DisplayName = displayName.SplitByUpperAndToLower();
        }

        public string DisplayName { get; }
    }
}