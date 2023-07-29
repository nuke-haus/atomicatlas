using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Instance<DefinitionClass, DataClass> : StaticInstance<DefinitionClass, DataClass> where DefinitionClass : Definition<DataClass>
{
    [SerializeField]
    protected DataClass data;

    public DataClass Data => data;

    protected Instance(DefinitionClass definition, string id) : base(definition, id)
    {
     
    }
}
