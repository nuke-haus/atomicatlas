using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.GAMEPLAY;

    private GameStateType nextState = GameStateType.GAMEPLAY;
    private IFeatureManager featureManager = DependencyInjector.Resolve<IFeatureManager>();

    public override GameStateType GetNextState()
    {
        return nextState;
    }

    public override void OnEnter()
    {
        Debug.Log("[GAMEPLAY STATE] Entering gameplay state");

        var definition = featureManager.GetDefinitionFromFeatureDefinition<WorldGenerationFeatureDefinition, ProvinceTypeDefinition, BiomeData>("TestBiome");
        var instance = definition.CreateInstance<ProvinceTypeDefinition, BiomeInstance>();
        var gridTest = instance.GenerateGrid();
        

    }

    public override void OnExit()
    {
        
    }
}
