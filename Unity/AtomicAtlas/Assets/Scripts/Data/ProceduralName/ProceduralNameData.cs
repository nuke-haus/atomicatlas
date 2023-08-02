using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine.Assertions;

[XmlRoot]
public class ProceduralNameData : IData
{
    [XmlElement("NamingSegment")]
    public List<NamingSegmentDefinition> NamingSegmentDefinitions;

    [XmlElement("NamingFormat")]
    public List<NamingFormatDefinition> NamingFormatDefinitions;

    public void Merge(IData data)
    {
        Assert.IsTrue(data.GetType() == typeof(ProceduralNameData), "Cannot merge data of different types");

        var nationData = (ProceduralNameData)data;
        NamingFormatDefinitions.AddRange(nationData.NamingFormatDefinitions);
        NamingSegmentDefinitions.AddRange(nationData.NamingSegmentDefinitions);
    }

    public NamingFormatDefinition GetRandomFormat(ProvinceType provinceType)
    {
        var valid = NamingFormatDefinitions.Where(x => !x.BlockedProvinceTypes.Where(y => provinceType.HasFlag(y)).Any()).ToList();
        return valid.GetRandom();
    }

    public string GetRandomSegmentString(string id, ProvinceType provinceType, bool isPlains)
    {
        var validData = NamingSegmentDefinitions.Where(x => x.ID == id 
                                                        && !x.BlockedProvinceTypes.Where(y => provinceType.HasFlag(y)).Any() 
                                                        && (x.ProvinceType == ProvinceType.NOTHRONE 
                                                            || ((x.ProvinceType != ProvinceType.PLAINS && provinceType.HasFlag(x.ProvinceType)) 
                                                            || (x.ProvinceType == ProvinceType.PLAINS && isPlains))));

      
        if (!validData.Any())
        {
            return null;
        }

        var strings = new List<string>();

        foreach (var data in validData)
        {
            strings.AddRange(data.Strings);
        }

        return strings.GetRandom();
    }
}
