using System;

namespace Atlas.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public Type Type
        {
            get;
            private set;
        }

        public InjectableAttribute(Type type)
        {
            Type = type;
        }
    }
}
    
