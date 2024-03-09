using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStrategy
{
    public bool IsStrategyDefinitionValid(StrategyDefinition strategyDefinition);
    public World GenerateWorld(StrategyDefinition strategyDefinition, IEnumerable<PlayerInfo> playerInfo);
}

public interface IWorldGenerationManager
{
    public World GenerateWorld(IStrategy strategy, StrategyDefinition strategyDefinition, IEnumerable<PlayerInfo> playerInfo);
}

[Injectable(typeof(IWorldGenerationManager))]
public class WorldGenerationManager : IWorldGenerationManager
{
    public WorldGenerationManager()
    {
       
    }

    public World GenerateWorld(IStrategy strategy, StrategyDefinition strategyDefinition, IEnumerable<PlayerInfo> playerInfo)
    {
        if (strategy == null)
        {
            Debug.LogError("Unable to generate world, generation strategy has not been set");
            return null;
        }

        if (strategyDefinition == null)
        {
            Debug.LogError("Unable to generate world, generation strategy definition has not been set");
            return null;
        }

        if (playerInfo == null || !playerInfo.Any())
        {
            Debug.LogError("Unable to generate world, player info has not been set");
            return null;
        }

        if (!strategy.IsStrategyDefinitionValid(strategyDefinition))
        {
            Debug.LogError("Unable to generate world, strategy definition is not valid for the strategy");
            return null;
        }

        return strategy.GenerateWorld(strategyDefinition, playerInfo);
    }
}
