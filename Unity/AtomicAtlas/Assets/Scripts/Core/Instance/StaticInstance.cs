using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class StaticInstance<DefinitionClass, DataClass> where DefinitionClass : Definition<DataClass>
{
    [SerializeField]
    protected DefinitionClass definition;

    [SerializeField]
    protected string id;

    public DefinitionClass Definition => definition;
    public string ID => id;

    protected StaticInstance(DefinitionClass definition, string instanceId) 
    {
        this.definition = definition;
        this.id = instanceId;
    }
}