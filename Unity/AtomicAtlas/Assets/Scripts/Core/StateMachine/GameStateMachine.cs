using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameStateMachine<StateType> where StateType : Enum
{
    private static Dictionary<StateType, Type> gameStateTypes;
    private Dictionary<StateType, GameState<StateType>> gameStates;
    private GameState<StateType> currentState;

    protected abstract StateType InitialState
    {
        get;
    }

    public void OnFixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        currentState.OnUpdate();
        
        if (currentState.CanExitState())
        {
            StateType nextStateType = currentState.GetNextState();
            GameState<StateType> nextState = gameStates[nextStateType];

            if (nextState == null)
            {
                throw new Exception("Unable to locate game state: " + nextStateType);
            }

            Debug.Log("[GAME STATE] Exiting state: " + currentState.GameStateType);
            currentState.OnExit();
            currentState = nextState;
            Debug.Log("[GAME STATE] Entering state: " + currentState.GameStateType);
            currentState.OnEnter();
        }
    }

    public GameStateMachine()
    {
        gameStates = new Dictionary<StateType, GameState<StateType>>();

        var gameStateType = typeof(GameState<StateType>);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => gameStateType.IsAssignableFrom(p));

        foreach (var classType in types)
        {
            GameState<StateType> state = (GameState<StateType>) Activator.CreateInstance(classType);
            gameStates.Add(state.GameStateType, state);
        }

        GameState<StateType> initialState = gameStates[InitialState];
        if (initialState == null)
        {
            throw new Exception("Unable to locate initial game state: " + InitialState);
        }

        Debug.Log("[GAME STATE] Entering state: " + InitialState);
        currentState = initialState;
        currentState.OnEnter();
    }
}
