using System.Collections.Generic;
using System.Xml.Serialization;

public class NamingSegmentDefinition
{
    [XmlElement]
    public string ID;
    [XmlElement]
    public ProvinceType ProvinceType = ProvinceType.NOTHRONE; // default nothrone flag is treated as global data that gets used on all province types
    [XmlElement("BlockedProvinceType")]
    public List<ProvinceType> BlockedProvinceTypes = new List<ProvinceType> { ProvinceType.NOTHRONE }; // filter against this value
    [XmlElement("String")]
    public List<string> Strings;

    public string GetRandomString()
    {
        return Strings.GetRandom();
    }
}
