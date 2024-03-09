using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot]
public class DefaultStrategyData : IStrategyData
{
    [XmlElement("StrategyDefinition")]
    public List<DefaultStrategyDefinition> StrategyDefinitions;

    public void Merge(IStrategyData data)
    {
        var strategyData = (DefaultStrategyData)data;
        StrategyDefinitions.AddRange(strategyData.StrategyDefinitions);
    }
}

public class DefaultStrategyDefinition : StrategyDefinition
{
    [XmlElement]
    public string SomeValue;
}

[Strategy(typeof(DefaultStrategyData))]
public class DefaultStrategy: IStrategy
{
    public bool IsStrategyDefinitionValid(StrategyDefinition strategyDefinition)
    {
        return strategyDefinition is DefaultStrategyDefinition;
    }

    public World GenerateWorld(StrategyDefinition definition, IEnumerable<PlayerInfo> players)
    {
        var strategyDefinition = (DefaultStrategyDefinition)definition;
        var world = new World(new Vector2(2048, 1024));

        var mainPlane = new WorldPlane("Default Plane", false);

        var n1 = mainPlane.CreateNode(new Vector2(0.5f, 0.5f));
        var n2 = mainPlane.CreateNode(new Vector2(0.6f, 0.6f));
        mainPlane.CreateConnection(n1, n2, false);

        world.AddPlane(mainPlane);

        return world;
    }
}
