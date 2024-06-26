using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Atlas.Data;

namespace Atlas.WorldGen.Strategies
{
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
        public bool SomeBoolValue;

        [XmlElement]
        public int SomeIntValue;

        [XmlElement]
        public IntRange SomeIntRange;

        [XmlElement]
        [IntRangeGroup("GROUP 1", 100)]
        public IntRange SomeGroupIntRange1 = new IntRange { Min=0, Max=10 };

        [XmlElement]
        [IntRangeGroup("GROUP 1", 100)]
        public IntRange SomeGroupIntRange2 = new IntRange { Min = 3, Max = 13 };

        [XmlElement]
        [IntRangeGroup("GROUP 2", 50)]
        public IntRange SomeGroupIntRange3 = new IntRange { Min = 2, Max = 12 };

        [XmlElement]
        [IntRangeGroup("GROUP 2", 50)]
        public IntRange SomeGroupIntRange4 = new IntRange { Min = 5, Max = 15 };
    }

    [Strategy("DEFAULT STRATEGY", typeof(DefaultStrategyData))]
    public class DefaultStrategy : IStrategy
    {
        public bool IsStrategyDefinitionValid(StrategyConfigDefinition strategyDefinition)
        {
            return strategyDefinition is DefaultStrategyConfigDefinition;
        }

        public World GenerateWorld(StrategyConfigDefinition definition, IEnumerable<PlayerInfo> players)
        {
            var strategyDefinition = (DefaultStrategyConfigDefinition)definition;
            var world = new World(new Vector2(2048, 1024));

            var mainPlane = new WorldPlane("MAIN PLANE", false);
            var cavePlane = new WorldPlane("CAVE PLANE", true);

            var n1 = mainPlane.CreateNode(new Vector2(0.1f, 0.2f));
            var n2 = mainPlane.CreateNode(new Vector2(0.2f, 0.3f));
            mainPlane.CreateConnection(n1, n2, false);

            var cn1 = cavePlane.CreateNode(new Vector2(0.7f, 0.7f));
            var cn2 = cavePlane.CreateNode(new Vector2(0.7f, 0.8f));
            cavePlane.CreateConnection(cn1, cn2, false);

            world.AddPlane(mainPlane);
            world.AddPlane(cavePlane);

            return world;
        }
    }
}