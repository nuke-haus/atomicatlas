using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public interface IGameStateManager
{
    void OnUpdate();
    void OnFixedUpdate();
}

[Injectable(typeof(IGameStateManager))]
public class GameStateManager : GameStateMachine<GameStateType>, IGameStateManager
{
    protected override GameStateType InitialState => GameStateType.BOOT;
}
