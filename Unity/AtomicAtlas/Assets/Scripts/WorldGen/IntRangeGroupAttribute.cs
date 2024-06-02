using System;

namespace Atlas.WorldGen
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IntRangeGroupAttribute : Attribute
    {
        public string GroupName
        {
            get;
            private set;
        }

        public int MaxValue
        {
            get;
            private set;
        }

        public IntRangeGroupAttribute(string name, int maxValue)
        {
            GroupName = name;
            MaxValue = maxValue;
        }
    } 
}
