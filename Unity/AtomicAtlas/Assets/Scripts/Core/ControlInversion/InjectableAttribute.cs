using System;

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
