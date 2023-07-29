using System;

[Serializable]
public class NationInstance : StaticInstance<NationDefinition, NationData>
{
    public NationInstance(NationDefinition definition, string id) : base(definition, id)
    {

    }
}