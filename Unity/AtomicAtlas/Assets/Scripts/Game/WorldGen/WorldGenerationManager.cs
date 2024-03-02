using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStrategy
{
    public bool IsStrategyDefinitionValid(StrategyDefinition strategyDefinition);
    public World GenerateWorld(StrategyDefinition strategyDefinition, List<PlayerInfo> playerInfo);
}

public interface IWorldGenerationManager
{
    public void SetStrategy(IStrategy strategy);
    public void SetStrategyDefinition(StrategyDefinition strategyDefinition);
    public void SetPlayerInfo(List<PlayerInfo> playerInfo);
    public World GenerateWorld();
}

[Injectable(typeof(IWorldGenerationManager))]
public class WorldGenerationManager : IWorldGenerationManager
{
    private IStrategy strategy;
    private StrategyDefinition strategyDefinition;
    private List<PlayerInfo> playerInfo;

    public WorldGenerationManager()
    {
       
    }

    public void SetStrategy(IStrategy strat)
    {
        strategy = strat;
    }

    public void SetStrategyDefinition(StrategyDefinition strategyDef)
    {
        strategyDefinition = strategyDef;
    }

    public World GenerateWorld()
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

    public void SetPlayerInfo(List<PlayerInfo> players)
    {
        playerInfo = players;
    }

    public void SetWorldGenerationStrategy(IStrategy generationStrategy)
    {
        strategy = generationStrategy;
    }

    public void SetStrategyConfiguration(StrategyDefinition strategyDef)
    {
        strategyDefinition = strategyDef;
    }
}
