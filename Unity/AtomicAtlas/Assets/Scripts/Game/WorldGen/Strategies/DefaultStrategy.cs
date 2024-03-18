using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot]
public class DefaultStrategyData : IStrategyData
{
    [XmlElement("StrategyConfigDefinition")]
    public List<DefaultStrategyConfigDefinition> DefaultStrategyConfigDefinitions;

    public IEnumerable<StrategyConfigDefinition> StrategyConfigDefinitions => DefaultStrategyConfigDefinitions;

    public void Merge(IStrategyData data)
    {
        var strategyData = (DefaultStrategyData)data;
        DefaultStrategyConfigDefinitions.AddRange(strategyData.DefaultStrategyConfigDefinitions);
    }
}

public class DefaultStrategyConfigDefinition : StrategyConfigDefinition
{
    [XmlElement]
    public string SomeValue;
}

[Strategy("DEFAULT STRATEGY", typeof(DefaultStrategyData))]
public class DefaultStrategy: IStrategy
{
    public bool IsStrategyDefinitionValid(StrategyConfigDefinition strategyDefinition)
    {
        return strategyDefinition is DefaultStrategyConfigDefinition;
    }

    public World GenerateWorld(StrategyConfigDefinition definition, IEnumerable<PlayerInfo> players)
    {
        var strategyDefinition = (DefaultStrategyConfigDefinition)definition;
        var world = new World(new Vector2(2048, 1024));

        var mainPlane = new WorldPlane("Default Plane", false);

        var n1 = mainPlane.CreateNode(new Vector2(0.5f, 0.5f));
        var n2 = mainPlane.CreateNode(new Vector2(0.6f, 0.6f));
        mainPlane.CreateConnection(n1, n2, false);

        world.AddPlane(mainPlane);

        return world;
    }
}
