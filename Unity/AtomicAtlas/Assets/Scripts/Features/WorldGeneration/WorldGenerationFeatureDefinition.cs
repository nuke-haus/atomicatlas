using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class WorldGenerationFeatureDefinition : FeatureDefinition
{
    [SerializeField]
    public List<ProvinceTypeDefinition> provinceTypes;

    [SerializeField]
    public List<GeneratorSettingsDefinition> generatorSettings;
}
