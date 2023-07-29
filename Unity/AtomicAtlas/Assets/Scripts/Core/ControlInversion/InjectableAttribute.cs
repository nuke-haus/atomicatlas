using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
