using System;

public enum Age
{
    EARLY,
    MIDDLE,
    LATE,
    ALL
}

[Serializable]
public class NationData
{
    public string Name;
    public int NumericID;
    public Age Age;
    public float WaterPercentage;
    public int CapRingSize;
    public ProvinceType CapProvinceType;
    public ProvinceType[] CapRingProvinceTypes;
}

[Serializable]
public class NationDefinition : Definition<NationData>
{
    public string Name => data.Name;
    public int NumericID => data.NumericID;
    public Age Age => data.Age;
    public float WaterPercentage => data.WaterPercentage;
    public int CapRingSize => data.CapRingSize;
    public ProvinceType CapProvinceType => data.CapProvinceType;
    public ProvinceType[] CapRingProvinceTypes => data.CapRingProvinceTypes;

    public NationDefinition(string id) : base(id)
    {

    }
}
