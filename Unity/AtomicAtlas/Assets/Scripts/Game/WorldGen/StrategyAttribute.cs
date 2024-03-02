using System;

[AttributeUsage(AttributeTargets.Class)]
public class StrategyAttribute : Attribute 
{
    public Type DataClassType
    {
        get;
        private set;
    }

    public StrategyAttribute(Type dataClassType)
    {
        DataClassType = dataClassType;
    }
}
