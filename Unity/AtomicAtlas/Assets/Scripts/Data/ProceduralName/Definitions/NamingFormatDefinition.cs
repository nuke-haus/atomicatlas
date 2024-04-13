using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atlas.Data
{
    public class NamingFormatDefinition
    {
        [XmlElement("Segment")]
        public List<string> Segments;
        [XmlElement("BlockedProvinceType")]
        public List<ProvinceType> BlockedProvinceTypes = new List<ProvinceType> { ProvinceType.NOTHRONE }; // filter against this value
    }
}