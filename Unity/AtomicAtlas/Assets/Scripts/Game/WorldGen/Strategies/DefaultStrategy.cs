using System.Collections.Generic;
using System.Xml.Serialization;

public class DefaultStrategyDefinition : StrategyDefinition
{
    [XmlElement]
    public string SomeValue;
}

[XmlRoot]
public class DefaultStrategyData : IStrategyData
{
    [XmlElement("StrategyDefinition")]
    public List<DefaultStrategyDefinition> StrategyDefinitions;

    public void Merge(IStrategyData data)
    {
        var strategyData = (DefaultStrategyData) data;
        StrategyDefinitions.AddRange(strategyData.StrategyDefinitions);
    }
}

[Strategy(typeof(DefaultStrategyData))]
public class DefaultStrategy: IStrategy
{
    public bool IsStrategyDefinitionValid(StrategyDefinition strategyDefinition)
    {
        return strategyDefinition is DefaultStrategyDefinition;
    }

    public World GenerateWorld(StrategyDefinition definition, List<PlayerInfo> players)
    {
        var strategyDefinition = (DefaultStrategyDefinition) definition;
        var world = new World();



        return world;
    }
}
