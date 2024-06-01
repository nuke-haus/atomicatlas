using System;

namespace Atlas.WorldGen
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class IntRangeGroupAttribute : Attribute
    {
        public string GroupName
        {
            get;
            private set;
        }

        public IntRangeGroupAttribute(string name)
        {
            GroupName = name;
        }
    } 
}
