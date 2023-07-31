using System;
using System.Xml.Serialization;

public enum Age
{
    EARLY,
    MIDDLE,
    LATE,
    ALL
}

public class NationDefinition
{
    [XmlElement]
    public string Name;
    [XmlElement]
    public int NumericID;
    [XmlElement]
    public Age Age;
    [XmlElement]
    public float WaterPercentage;
    [XmlElement]
    public int CapRingSize;
    [XmlElement]
    public ProvinceType CapProvinceType;
    [XmlElement("CapRingProvinceType")]
    public ProvinceType[] CapRingProvinceTypes;
}
