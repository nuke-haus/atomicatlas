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
        public IntRange SomeGroupIntRange1 = new IntRange { Min = 0, Max = 10 };

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

            GenerateSimpleWorld(mainPlane, 8, 8);
            GenerateSimpleWorld(cavePlane, 8, 8);

            world.AddPlane(mainPlane);
            world.AddPlane(cavePlane);

            return world;
        }

        private void GenerateSimpleWorld(WorldPlane plane, int w, int h)
        {
            var dict = new Dictionary<Vector2, Node>();
            float wFloat = (float)w;
            float hFloat = (float)h;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    float iFloat = (float)i;
                    float jFloat = (float)j;
                    float x = Mathf.Clamp(iFloat / wFloat, 0f, 1f);
                    float y = Mathf.Clamp(jFloat / hFloat, 0f, 1f);
                    var node = plane.CreateNode(new Vector2(x, y));
                    dict.Add(new Vector2(x, y), node);
                }
            }

            foreach (var key in dict.Keys)
            {
                var node = dict[key];
                var xRight = key.x + (1f / wFloat);
                var yUp = key.y + (1f / hFloat);
                bool rightWrap = false;
                bool upWrap = false;

                if (xRight >= 1f)
                {
                    xRight = 0f;
                    rightWrap = true;
                }
                if (yUp >= 1f)
                {
                    yUp = 0f;
                    upWrap = true;
                }

                var right = new Vector2(xRight, key.y);
                var up = new Vector2(key.x, yUp);
                var diag = new Vector2(xRight, yUp);

                plane.CreateConnection(dict[right], node, rightWrap);
                plane.CreateConnection(dict[up], node, upWrap);
                plane.CreateConnection(dict[diag], node, rightWrap || upWrap);
            }
        }
    }
}