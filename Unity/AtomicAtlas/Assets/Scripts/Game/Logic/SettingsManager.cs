
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public interface ISettingsManager
{
    public IStrategy Strategy { get; }
    public StrategyConfigDefinition StrategyConfigDefinition { get; }
    public IEnumerable<PlayerInfo> AllPlayerInfo { get; }
    public bool IsDisciples { get; }

    public void SetDisciples(bool disciples);
    public void UpdatePlayerCount(int count);
    public IEnumerable<Type> GetStrategyTypes();
    public void SetActiveStrategy(Type strategyType);
    public void SetActiveStrategyConfigDefinition(StrategyConfigDefinition activeStrategyDefinition);
}

[Injectable(typeof(ISettingsManager))]
public class SettingsManager : ISettingsManager
{
    public IStrategy Strategy { get; private set; }
    public StrategyConfigDefinition StrategyConfigDefinition { get; private set; }
    public bool IsDisciples { get; private set; }
    public IEnumerable<PlayerInfo> AllPlayerInfo => allPlayerInfo;

    private IDataManager dataManager;
    private Type strategyType;
    private List<PlayerInfo> allPlayerInfo;

    public SettingsManager()
    {
        dataManager = DependencyInjector.Resolve<IDataManager>();
        strategyType = typeof(DefaultStrategy);
        Strategy = (IStrategy)Activator.CreateInstance(strategyType);  
        allPlayerInfo = new List<PlayerInfo>();
        IsDisciples = false;
    }

    public void UpdatePlayerCount(int newCount)
    {
        var nationData = dataManager.GetData<NationData>();
        if (newCount > allPlayerInfo.Count)
        {
            while (allPlayerInfo.Count < newCount)
            {
                allPlayerInfo.Add(new PlayerInfo(0, nationData.Nations[0]));
            }
        }
        else if (newCount < allPlayerInfo.Count)
        {
            while (allPlayerInfo.Count > newCount)
            {
                allPlayerInfo.RemoveAt(allPlayerInfo.Count - 1);
            }
        }
    }

    public void SetDisciples(bool isDisciples)
    {
        IsDisciples = isDisciples;
    }

    public void SetActiveStrategy(Type type)
    {
        if (!GetStrategyTypes().Contains(type))
        {
            Debug.LogError("Invalid strategy type");
        }

        strategyType = type;
        Strategy = (IStrategy)Activator.CreateInstance(type);
    }

    public IEnumerable<Type> GetStrategyTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(StrategyAttribute)));
    }

    public Dictionary<string, object> GetStrategyDefinitionFields()
    {
        var type = StrategyConfigDefinition.GetType();
        var result = new Dictionary<string, object>();

        foreach (var field in type.GetFields().Where(f => f.IsPublic))
        {
            result.Add(field.Name, field.GetValue(StrategyConfigDefinition));
        }

        return result;
    }

    public void SetActiveStrategy(IStrategy activeStrategy)
    {
        Strategy = activeStrategy;
    }

    public void SetActiveStrategyConfigDefinition(StrategyConfigDefinition definition)
    {
        StrategyConfigDefinition = definition;
    }
}
