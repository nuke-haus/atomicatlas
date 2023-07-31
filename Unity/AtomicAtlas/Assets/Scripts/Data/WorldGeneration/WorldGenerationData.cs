using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

[XmlRoot]
public class WorldGenerationData : IData
{
    [XmlElement("GeneratorConfig")]
    public List<GeneratorConfigDefinition> GeneratorConfigs;

    public void Merge(IData data)
    {
        Assert.IsTrue(data.GetType() == typeof(WorldGenerationData), "Cannot merge data of different types");

        var worldGenData = (WorldGenerationData)data;
        GeneratorConfigs.AddRange(worldGenData.GeneratorConfigs);
    }
}
