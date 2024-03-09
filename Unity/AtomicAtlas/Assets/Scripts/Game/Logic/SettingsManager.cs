
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public interface ISettingsManager
{
    public IStrategy ActiveStrategy { get; }
    public StrategyDefinition ActiveStrategyDefinition { get; }
    public IEnumerable<PlayerInfo> AllPlayerInfo { get; }

    public void AddPlayerInfo(PlayerInfo playerInfo);
    public void RemovePlayerInfo(PlayerInfo playerInfo);
    public IEnumerable<Type> GetStrategyTypes();
    public void SetActiveStrategy(Type strategyType);
    public void SetActiveStrategyDefinition(StrategyDefinition activeStrategyDefinition);
}

[Injectable(typeof(ISettingsManager))]
public class SettingsManager : ISettingsManager
{
    public IStrategy ActiveStrategy => strategy;
    public StrategyDefinition ActiveStrategyDefinition => strategyDefinition;
    public IEnumerable<PlayerInfo> AllPlayerInfo => allPlayerInfo;

    private IDataManager dataManager;
    private IStrategy strategy;
    private Type strategyType;
    private StrategyDefinition strategyDefinition;
    private List<PlayerInfo> allPlayerInfo;

    public SettingsManager()
    {
        dataManager = DependencyInjector.Resolve<IDataManager>();
        strategyType = typeof(DefaultStrategy);
        strategy = (IStrategy)Activator.CreateInstance(strategyType);  
    }

    public void AddPlayerInfo(PlayerInfo playerInfo)
    {
        allPlayerInfo.Add(playerInfo);
    }

    public void RemovePlayerInfo(PlayerInfo playerInfo)
    {
        allPlayerInfo.Remove(playerInfo);
    }

    public void SetActiveStrategy(Type type)
    {
        if (!GetStrategyTypes().Contains(type))
        {
            Debug.LogError("Invalid strategy type");
        }

        strategyType = type;
        strategy = (IStrategy)Activator.CreateInstance(type);
    }

    public IEnumerable<Type> GetStrategyTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(StrategyAttribute)));
    }

    public Dictionary<string, object> GetStrategyDefinitionFields()
    {
        var type = strategyDefinition.GetType();
        var result = new Dictionary<string, object>();

        foreach (var field in type.GetFields().Where(f => f.IsPublic))
        {
            result.Add(field.Name, field.GetValue(strategyDefinition));
        }

        return result;
    }

    public void SetActiveStrategy(IStrategy activeStrategy)
    {
        strategy = activeStrategy;
    }

    public void SetActiveStrategyDefinition(StrategyDefinition definition)
    {
        strategyDefinition = definition;
    }
}
