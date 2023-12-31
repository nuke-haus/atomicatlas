﻿using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.Assertions;

[XmlRoot]
public class NationData : IData
{
    [XmlElement("Nation")]
    public List<NationDefinition> Nations;

    public void Merge(IData data)
    {
        Assert.IsTrue(data.GetType() == typeof(NationData), "Cannot merge data of different types");

        var nationData = (NationData)data;
        Nations.AddRange(nationData.Nations);
    }
}
