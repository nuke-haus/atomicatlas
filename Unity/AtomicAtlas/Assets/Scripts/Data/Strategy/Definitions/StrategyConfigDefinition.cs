
using System.Xml.Serialization;

namespace Atlas.Data
{
    public abstract class StrategyConfigDefinition
    {
        [XmlElement]
        public string Name;
    }
}