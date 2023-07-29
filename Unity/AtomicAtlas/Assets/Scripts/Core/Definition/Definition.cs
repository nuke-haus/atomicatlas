using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class Definition<DataClass>
{
    [SerializeField]
    protected string id;

    [SerializeField]
    protected DataClass data;

    public string ID => id;

    public InstanceClass CreateInstance<DefinitionClass, InstanceClass>() where DefinitionClass : Definition<DataClass> where InstanceClass : StaticInstance<DefinitionClass, DataClass> 
    {
        var classType = typeof(InstanceClass);
        var ctors = classType.GetConstructors();
        var constructor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 2 && ctor.GetParameters()[0].ParameterType == typeof(DefinitionClass));
        Assert.IsNotNull(constructor, "Unable to find instance constructor for " + classType.Name);

        return (InstanceClass) constructor.Invoke(new object[] { this, Guid.NewGuid().ToString() });
    }

    public Definition(string id)
    {
        this.id = id;
    }
}
