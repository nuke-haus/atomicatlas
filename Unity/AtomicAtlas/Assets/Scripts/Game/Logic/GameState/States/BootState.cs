using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class BootState : GameState<GameStateType>
{
    public override GameStateType GameStateType => GameStateType.BOOT;

    public override GameStateType GetNextState()
    {
        return GameStateType.MAIN_MENU;
    }

    public override bool CanExitState()
    {
        return true;
    }

    public override void OnEnter()
    {
        Debug.Log("[BOOT STATE] Booting game");

        DependencyInjector.Resolve<IFeatureManager>(); // Construct the feature manager so it loads features from disk
    }
}
