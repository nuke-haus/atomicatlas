using System;

[Serializable]
public class GeneratorSettingsData
{
    // TODO: Copy over all generator settings stuff from mapnuke
}

[Serializable]
public class GeneratorSettingsDefinition : Definition<GeneratorSettingsData>
{  
    public GeneratorSettingsDefinition(string id) : base(id)
    {

    }
}
