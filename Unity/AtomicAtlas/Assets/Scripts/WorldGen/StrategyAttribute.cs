using System;

namespace Atlas.WorldGen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StrategyAttribute : Attribute
    {
        public string DisplayName
        {
            get;
            private set;
        }

        public Type DataClassType
        {
            get;
            private set;
        }

        public StrategyAttribute(string name, Type dataClassType)
        {
            DisplayName = name;
            DataClassType = dataClassType;
        }
    } 
}
